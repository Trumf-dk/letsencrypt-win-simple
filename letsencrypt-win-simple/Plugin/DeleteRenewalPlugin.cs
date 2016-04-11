using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Serilog;
using Microsoft.Web.Administration;

namespace LetsEncrypt.ACME.Simple
{
    public class DeleteRenewalPlugin : Plugin
    {
        private const string ClientName = "letsencrypt-win-simple";

        public override string Name => "DeleteRenewalPlugin";

        public override void PrintMenu()
        {
            Console.WriteLine(" R: Delete Renewal");
        }

        public override void HandleMenuResponse(string response, List<Target> targets)
        {
            if (response == "r")
            {
                Console.WriteLine("Running DeleteRenewal Plugin");
                Log.Information("Running DeleteRenewal Plugin");

                GetRenewals();
            }
        }

        public static void GetRenewals()
        {
            try
            {
                var _settings = new Settings(ClientName, Program.Options.BaseUri);
                var renewals = _settings.LoadRenewals();
                Console.WriteLine();
                Console.WriteLine("Renewals:");
                var i = 0;
                foreach (var renewal in renewals.OrderBy(o => o.Binding.Host))
                {
                    i++;
                    var txt = ("  " + i + ": " + renewal.Binding.Host + (!String.IsNullOrEmpty(renewal.San) && renewal.San.ToLowerInvariant() == "true" ? " (SAN) " : " ")).PadRight(40);
                    Console.WriteLine(txt + "Renewal date: " + renewal.Date.ToShortDateString());
                    if (renewal.Binding.AlternativeNames != null && renewal.Binding.AlternativeNames.Any())
                    {
                        foreach (var item in renewal.Binding.AlternativeNames)
                        {
                            Console.WriteLine("    - " + item);
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("  Enter Id of Renewal to delete");
                var selectedRenewal = Console.ReadLine();
                int x;
                string selectedRenewalName = "";
                if (int.TryParse(selectedRenewal, out x))
                {
                    var renewalList = new List<ScheduledRenewal>();
                    var j = 0;
                    foreach (var renewal in renewals.OrderBy(o => o.Binding.Host))
                    {
                        j++;
                        if (j != x)
                        {
                            renewalList.Add(renewal);
                        }
                        else
                            selectedRenewalName = renewal.Binding.Host + (!String.IsNullOrEmpty(renewal.San) && renewal.San.ToLowerInvariant() == "true" ? " (SAN) " : " ");
                    }

                    Console.WriteLine();
                    Console.WriteLine("  Are you sure you wan't to delete renewal: " + selectedRenewalName + "?");
                    if (Program.PromptYesNo())
                    {
                        _settings.SaveRenewals(renewalList);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error encountered while loading renewals. Error: {@ex}", ex);
                throw new Exception(ex.Message);
            }
        }

        public override List<Target> GetTargets()
        {
            var result = new List<Target>();

            return result;
        }

        public override List<Target> GetSites()
        {
            var result = new List<Target>();

            return result;
        }

        public override void Install(Target target, string pfxFilename, X509Store store, X509Certificate2 certificate)
        {
            Console.WriteLine(" WARNING: Unable to configure server software.");
        }

        public override void Install(Target target)
        {
            Console.WriteLine(" WARNING: Unable to configure server software.");
        }

        public override void Auto(Target target)
        {
            Console.WriteLine("Auto isn't supported for Overview Plugin");
        }

        public override void Renew(Target target)
        {
            Console.WriteLine("Renew isn't supported for Overview Plugin");
        }
    }
}