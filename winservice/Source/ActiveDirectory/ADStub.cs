using Aula_Dagtilbud_AD_Integration.DTO;
using Serilog;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Aula_Dagtilbud_AD_Integration.ActiveDirectory
{
    public class ADStub
    {
        private static string adUrl = Settings.GetStringValue("adUrl");
        private static string adRoot = Settings.GetStringValue("adRoot");
        private static string adUsername = Settings.GetStringValue("adUsername");
        private static string adPassword = Settings.GetStringValue("adPassword");
        private static string uuidAttribute = Settings.GetStringValue("uuidAttribute");
        private static bool adRemoteEnabled = !string.IsNullOrEmpty(adUrl) && !string.IsNullOrEmpty(adRoot) && !string.IsNullOrEmpty(adUsername) && !string.IsNullOrEmpty(adPassword);

        public ILogger Logger { get; set; }

        public List<UserDTO> GetUsersWithRole(string groupIdentifier)
        {
            Logger.Information("Looking up users in group: " + groupIdentifier);
            List<UserDTO> result = new List<UserDTO>();

            using (var context = GetPrincipalContext())
            {
                using (GroupPrincipal group = GroupPrincipal.FindByIdentity(context, groupIdentifier))
                {
                    if (group == null)
                    {
                        Logger.Error("Group does not exist: " + groupIdentifier);
                    }
                    else
                    {
                        // argument to GetMembers indicate we want direct members, not indirect members
                        foreach (Principal member in group.GetMembers(false))
                        {
                            var entry = new UserDTO();
                            entry.userId = member.SamAccountName;

                            if (!string.IsNullOrEmpty(uuidAttribute))
                            {
                                var de = member.GetUnderlyingObject() as DirectoryEntry;
                                if (de != null)
                                {
                                    var attributeValue = de.Properties[uuidAttribute];
                                    entry.uuid = attributeValue.Value?.ToString().ToLower();

                                    if (entry.uuid != null && entry.uuid.Length != 36)
                                    {
                                        Logger.Warning("user " + member.Name + " has a non UUID (" + entry.uuid + ") in attribute " + uuidAttribute + " defaulting to ObjectGuid");
                                        entry.uuid = null;
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(entry.uuid))
                            {
                                entry.uuid = member.Guid?.ToString().ToLower();
                            }

                            if (!string.IsNullOrEmpty(entry.userId) && !string.IsNullOrEmpty(entry.uuid))
                            {
                                result.Add(entry);
                            }
                            else
                            {
                                Logger.Error("Invalid member: " + entry.userId + " / " + entry.uuid);
                            }
                        }
                    }
                }
            }

            return result;
        }

        private PrincipalContext GetPrincipalContext()
        {
            if (adRemoteEnabled)
            {
                return new PrincipalContext(ContextType.Domain, adUrl, adRoot, adUsername, adPassword);
            }

            return new PrincipalContext(ContextType.Domain);
        }
    }
}