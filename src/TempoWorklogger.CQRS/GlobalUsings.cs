global using MediatR;
global using TempoWorklogger.Contract.Services;
global using TempoWorklogger.Model.Db;

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using unitResult = Maya.Ext.Rop.Result<Maya.Ext.Unit, System.Exception>;
global using importMapResult = Maya.Ext.Rop.Result<TempoWorklogger.Model.Db.ImportMap, System.Exception>;
global using importMapsResult = Maya.Ext.Rop.Result<System.Collections.Generic.IEnumerable<TempoWorklogger.Model.Db.ImportMap>, System.Exception>;
global using worklogsResult = Maya.Ext.Rop.Result<System.Collections.Generic.IEnumerable<TempoWorklogger.Model.Db.Worklog>, System.Exception>;
