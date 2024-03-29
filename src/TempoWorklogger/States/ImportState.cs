﻿using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.States
{
    public class ImportState : IDisposable
    {
        public FileInfo File { get; set; }

        public ImportMap ImportMap { get; set; }

        public List<Result<Worklog, (Exception Exception, int RowNr)>> WorklogsResults { get; set; }

        public List<Result<(Worklog, WorklogResponse), (Worklog, Exception)>> WorklogResponseResults { get; set; } = new();

        public void Dispose()
        {
            File = new();
            ImportMap = new();
            WorklogsResults = new();
            WorklogResponseResults = new();
        }
    }
}
