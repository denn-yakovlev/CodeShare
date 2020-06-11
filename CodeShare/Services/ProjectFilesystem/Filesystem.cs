using CodeShare.Services.DatabaseInteractor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeShare.Services.ProjectFilesystem
{
    public class Filesystem : IFilesystemService
    {

        public Directory Root { get; } = Directory.Root;

        private IDatabaseInteractor dbInteractor;

        public Filesystem(IDatabaseInteractor dbInteractor)
        {
            this.dbInteractor = dbInteractor;
        }

        public void Mount(object sender, ConnToProjectEventArgs e)
        {
            var project = dbInteractor.Projects.Read(e.ProjectId);
            foreach (var file in project.Files)
            {
                var pathParts = file.Path.Split("/", StringSplitOptions.RemoveEmptyEntries);
                Directory previous = Root;
                for (int i = 0; i < pathParts.Length - 1; i++)
                {
                    previous = previous.Dir(pathParts[i]);
                }
                previous.File(pathParts.Last());
            }

        }

        private IEnumerable<File> GetAllFilesFrom(Directory dir)
        {
            var queue = new Queue<IFilesystemItem>();
            var result = new Queue<File>();
            queue.Enqueue(dir);
            while (queue.Count != 0)
            {
                if (queue.Peek() is Directory d)
                {
                    foreach (var item in d.Items)
                        queue.Enqueue(item);                 
                }
                else
                {
                    result.Enqueue(queue.Peek() as File);
                }
                queue.Dequeue();
            }
            return result;
        }

        public void Unmount(object sender, ConnToProjectEventArgs e)
        {
            var project = dbInteractor.Projects.Read(e.ProjectId);
            var projectDir = Root.Items.SingleOrDefault(item => item.Name == e.ProjectId) as Directory;
            var projectFiles = GetAllFilesFrom(projectDir);
            if (projectDir != null)
            {
                Root.Items.Remove(projectDir);
            }          
            dbInteractor.Projects.Update(
                e.ProjectId,
                new Models.Project
                {
                    Name = project.Name,
                    Description = project.Description,
                    Owner = project.Owner,
                    Files = projectFiles.Select(f => f.ToModel())
                }
            ); 
           
        }
    }
}
