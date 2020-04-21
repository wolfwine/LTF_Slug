/*
 * Created by SharpDevelop.
 * User: Etienne
 * Date: 22/11/2017
 * Time: 16:43
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System.Collections.Generic;
using Verse;

namespace LTF_Slug
{
	public class HeDiffCompProperties_ResetAbilities : HediffCompProperties
	{
        //what
        //public List<AbilityUser.AbilityDef> abilitiesToReset;

        public bool debug = false;

        public HeDiffCompProperties_ResetAbilities()
		{
			this.compClass = typeof(HeDiffComp_ResetAbilities);
		}
	}
}
