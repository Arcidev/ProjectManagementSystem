using BL.DTO;
using BL.Facades;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ProjectManagementSystem.Resources;
using Shared.Enums;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ProjectFacade projectFacade;
        private readonly TaskFacade taskFacade;

        public ReportController(ProjectFacade projectFacade, TaskFacade taskFacade)
        {
            this.projectFacade = projectFacade;
            this.taskFacade = taskFacade;
        }

        /// <summary>
        /// Creates report for all projects based on state
        /// </summary>
        /// <param name="state">State of project</param>
        /// <returns>Excel file with projects</returns>
        [HttpGet("project/{state}")]
        public async Task<FileContentResult> Get(ProjectState state)
        {
            var projects = await projectFacade.GetProjects(state);

            var workBook = new XSSFWorkbook();
            var sheet = workBook.CreateSheet();
            CreateProjectHeader(sheet);

            var rowIdx = 1;
            foreach (var project in projects)
            {
                WriteSubProject(sheet, project, rowIdx++);
                foreach (var subProject in project.SubProjects)
                    WriteSubProject(sheet, subProject, rowIdx++);
            }

            using var memoryStream = new MemoryStream();
            workBook.Write(memoryStream);
            return File(memoryStream.ToArray(), "application/octet-stream", $"ProjectsReport_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        /// <summary>
        /// Creates report for all tasks based on state
        /// </summary>
        /// <param name="state">State of task</param>
        /// <returns>Excel file with tasks</returns>
        [HttpGet("task/{state}")]
        public async Task<FileContentResult> Get(TaskState state)
        {
            var tasks = await taskFacade.GetTasks(state);

            var workBook = new XSSFWorkbook();
            var sheet = workBook.CreateSheet();
            var rowIdx = 0;
            var header = sheet.CreateRow(rowIdx++);
            CreateTaskHeader(header, 0);

            foreach (var task in tasks)
            {
                WriteTask(sheet.CreateRow(rowIdx), task, 0, ref rowIdx);
                rowIdx++;
            }

            using var memoryStream = new MemoryStream();
            workBook.Write(memoryStream);
            return File(memoryStream.ToArray(), "application/octet-stream", $"TaskReport_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx");
        }

        private void WriteSubProject(ISheet sheet, SubProjectDTO project, int rowIdx)
        {
            var row = sheet.CreateRow(rowIdx);
            var index = 0;

            row.CreateCell(index++).SetCellValue(project.Code);
            row.CreateCell(index++).SetCellValue(project.Name);
            row.CreateCell(index++).SetCellValue(project.State.ToString());
            row.CreateCell(index++).SetCellValue(project.StartDate);
            row.CreateCell(index++).SetCellValue(project.FinishDate?.ToString() ?? string.Empty);
            row.CreateCell(index++).SetCellValue(project is ProjectDTO dto ? string.Join(", ", dto.SubProjects.Select(x => x.Code)) : string.Empty);

            foreach (var task in project.Tasks)
            {
                row = sheet.CreateRow(++rowIdx);
                WriteTask(row, task, index, ref rowIdx);
            }
        }

        private void WriteTask(IRow row, TaskDTO task, int headerOffset, ref int rowIndex)
        {
            WriteSubTask(row, task, headerOffset);

            foreach (var subTask in task.SubTasks)
            {
                row = row.Sheet.CreateRow(++rowIndex);
                WriteSubTask(row, subTask, headerOffset);
            }
        }

        private void WriteSubTask(IRow row, SubTaskDTO subTask, int headerOffset)
        {
            row.CreateCell(headerOffset++).SetCellValue(subTask.Name);
            row.CreateCell(headerOffset++).SetCellValue(subTask.Description);
            row.CreateCell(headerOffset++).SetCellValue(subTask.State.ToString());
            row.CreateCell(headerOffset++).SetCellValue(subTask.StartDate);
            row.CreateCell(headerOffset++).SetCellValue(subTask.FinishDate?.ToString() ?? string.Empty);
            row.CreateCell(headerOffset++).SetCellValue(subTask is TaskDTO dto ? string.Join(", ", dto.SubTasks.Select(x => x.Name)) : string.Empty);
        }

        private void CreateProjectHeader(ISheet sheet)
        {
            var header = sheet.CreateRow(0);
            var index = 0;

            header.CreateCell(index++).SetCellValue(Texts.ProjectCode);
            header.CreateCell(index++).SetCellValue(Texts.ProjectName);
            header.CreateCell(index++).SetCellValue(Texts.ProjectState);
            header.CreateCell(index++).SetCellValue(Texts.ProjectStart);
            header.CreateCell(index++).SetCellValue(Texts.ProjectFinish);
            header.CreateCell(index++).SetCellValue(Texts.ProjectSubProjects);

            CreateTaskHeader(header, index);
        }

        private void CreateTaskHeader(IRow row, int headerOffset)
        {
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskName);
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskDescription);
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskState);
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskStart);
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskFinish);
            row.CreateCell(headerOffset++).SetCellValue(Texts.TaskSubTasks);
        }
    }
}
