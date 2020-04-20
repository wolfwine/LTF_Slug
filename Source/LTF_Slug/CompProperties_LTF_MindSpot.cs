/*
 * Created by SharpDevelop.
 * User: Etienne
 * Date: 22/11/2017
 * Time: 16:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Verse;

namespace LTF_Slug
{
	public class CompProperties_LTF_MindSpot : CompProperties
	{
        public float range = 3f;

        public bool affectsHumanlike = false;
        public bool affectsAnimals = false;
        public bool affectsMechanoids = false;

        public bool affectsColonists = false;
        public bool affectsNeutralOrFriends = false;
        public bool affectsEnemies = false;

        public int lifeSpan = 2000;
        public int hediffAppliedLimit = 15;

        public CompProperties_LTF_MindSpot()
		{
			this.compClass = typeof(Comp_LTF_MindSpot);
		}
	}
}
