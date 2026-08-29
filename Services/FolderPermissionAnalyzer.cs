using System.Security.AccessControl;
using CdiskClean.Models;

namespace CdiskClean.Services;

public class FolderPermissionAnalyzer
{
    public FolderPermissionInfo Analyze(string path)
    {
        if (!Directory.Exists(path))
            return new FolderPermissionInfo { Path = path, Status = "目录不存在", CanRead = false };

        try
        {
            var directory = new DirectoryInfo(path);
            var security = directory.GetAccessControl(AccessControlSections.Access | AccessControlSections.Owner);
            var rules = security.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            var canRead = CanEnumerate(path);
            return new FolderPermissionInfo
            {
                Path = path,
                Status = canRead ? "可读取" : "无读取权限",
                Owner = security.GetOwner(typeof(System.Security.Principal.NTAccount))?.Value,
                RuleCount = rules.Count,
                CanRead = canRead
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FolderPermissionInfo { Path = path, Status = "无权限", CanRead = false, ErrorMessage = ex.Message };
        }
        catch (Exception ex)
        {
            return new FolderPermissionInfo { Path = path, Status = "读取失败", CanRead = false, ErrorMessage = ex.Message };
        }
    }

    private static bool CanEnumerate(string path)
    {
        try
        {
            using var enumerator = Directory.EnumerateFileSystemEntries(path).GetEnumerator();
            enumerator.MoveNext();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
