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
	public class CompProperties_LTF_MindFlaySpot : CompProperties
	{
        public float range = 3f;

        public CompProperties_LTF_MindFlaySpot()
		{
			this.compClass = typeof(Comp_LTF_MindFlaySpot);
		}
	}
}
