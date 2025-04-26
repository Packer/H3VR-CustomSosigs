using FistVR;

namespace CustomSosigLoader;

public class CSL_WearablePasser : SosigWearblePasser
{
    public Damage.DamageClass[] damageBlacklist;

    public new void Damage(Damage d)
    {
        for (int i = 0; i < damageBlacklist.Length; i++)
        {
            if (d.Class == damageBlacklist[i])
            {
                //Stop all damage but still pass damage to alert sosig
                d.Dam_Blinding = 0;
                d.Dam_Cutting = 0;
                d.Dam_Chilling = 0;
                d.Dam_EMP = 0;
                d.Dam_Piercing = 0;
                d.Dam_Stunning = 0;
                d.Dam_Thermal = 0;
                d.Dam_TotalKinetic = 0;
                d.Dam_TotalEnergetic = 0;
                break;
            }
        }

        this.W.Damage(d);
    }
}