using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    /// <summary>
    /// 此类用于支持ef设计时工具的运行。
    /// </summary>
    /// <remarks>
    /// 当在命令行执行dotnet ef migrations add xxx时，
    /// ef 工具需要从本项目中找到一个DbContext的派生类，并创建其实例。
    /// 可以通过多种方式创建其实例，以下页面介绍了这些方式：
    /// https://docs.microsoft.com/zh-cn/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli
    /// </remarks>
    internal class DbContextExFactory : IDesignTimeDbContextFactory<XDbContext>
    {
        public XDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<XDbContext>();
            optionsBuilder.UseMySql("server=192.168.1.99;port=3306;user=root;password=abc123;database=test", ServerVersion.Parse("5.7.27-log"));

            return new XDbContext(optionsBuilder.Options);
        }
    }
}
