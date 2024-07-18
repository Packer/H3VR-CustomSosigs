using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TNHFramework;
using FistVR;

namespace CustomSosigLoader
{
    public class TnHFrameworkLoader
    {
        public static void AddSosigEnemyTemplateToTnHFramework(SosigEnemyTemplate template)
        {
            LoadedTemplateManager.SosigIDDict.Add(template.DisplayName, (int)template.SosigEnemyID);
        }
    }
}