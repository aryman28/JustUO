using System;
using System.Collections;
using Server;
using Server.Items;

namespace Server.Commands
{
	public class GenTeleporter
	{
		public GenTeleporter()
		{
		}

		public static void Initialize()
		{
			CommandSystem.Register( "TelGen", AccessLevel.Administrator, new CommandEventHandler( GenTeleporter_OnCommand ) );
		}

		[Usage( "TelGen" )]
		[Description( "Generates world/dungeon teleporters for all facets." )]
		public static void GenTeleporter_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Generating teleporters, please wait." );

			int count = new TeleportersCreator().CreateTeleporters();

			count += new SHTeleporter.SHTeleporterCreator().CreateSHTeleporters();

			e.Mobile.SendMessage( "Teleporter generating complete. {0} teleporters were generated.", count );
		}

		public class TeleportersCreator
		{
			private int m_Count;
			
			public TeleportersCreator()
			{
			}

			private static Queue m_Queue = new Queue();

			public static bool FindTeleporter( Map map, Point3D p )
			{
				IPooledEnumerable eable = map.GetItemsInRange( p, 0 );

				foreach ( Item item in eable )
				{
					if ( item is Teleporter && !(item is KeywordTeleporter) && !(item is SkillTeleporter) )
					{
						int delta = item.Z - p.Z;

						if ( delta >= -12 && delta <= 12 )
							m_Queue.Enqueue( item );
					}
				}

				eable.Free();

				while ( m_Queue.Count > 0 )
					((Item)m_Queue.Dequeue()).Delete();

				return false;
			}

			public void CreateTeleporter( Point3D pointLocation, Point3D pointDestination, Map mapLocation, Map mapDestination, bool back )
			{
				if ( !FindTeleporter( mapLocation, pointLocation ) )
				{
					m_Count++;
				
					Teleporter tel = new Teleporter( pointDestination, mapDestination );

					tel.MoveToWorld( pointLocation, mapLocation );
				}

				if ( back && !FindTeleporter( mapDestination, pointDestination ) )
				{
					m_Count++;

					Teleporter telBack = new Teleporter( pointLocation, mapLocation );

					telBack.MoveToWorld( pointDestination, mapDestination );
				}
			}

			public void CreateTeleporter( int xLoc, int yLoc, int zLoc, int xDest, int yDest, int zDest, Map map, bool back )
			{
				CreateTeleporter( new Point3D( xLoc, yLoc, zLoc ), new Point3D( xDest, yDest, zDest ), map, map, back);
			}

			public void CreateTeleporter( int xLoc, int yLoc, int zLoc, int xDest, int yDest, int zDest, Map mapLocation, Map mapDestination, bool back )
			{
				CreateTeleporter( new Point3D( xLoc, yLoc, zLoc ), new Point3D( xDest, yDest, zDest ), mapLocation, mapDestination, back);
			}

			public void DestroyTeleporter( int x, int y, int z, Map map )
			{
				Point3D p = new Point3D( x, y, z );
				IPooledEnumerable eable = map.GetItemsInRange( p, 0 );

				foreach ( Item item in eable )
				{
					if ( item is Teleporter && !(item is KeywordTeleporter) && !(item is SkillTeleporter) && item.Z == p.Z )
						m_Queue.Enqueue( item );
				}

				eable.Free();

				while ( m_Queue.Count > 0 )
					((Item)m_Queue.Dequeue()).Delete();
			}

			public void CreateTeleportersMap( Map map )
			{
      }
      
			public void CreateTeleportersMap2( Map map )
			{

                                // Czarna Rzeka
				CreateTeleporter( 224, 606, 0, 2087, 224, 14, map, false );
				CreateTeleporter( 224, 605, 0, 2087, 224, 14, map, false );
				CreateTeleporter( 224, 604, 0, 2087, 223, 14, map, false );
				CreateTeleporter( 224, 603, 0, 2087, 223, 14, map, false );
				CreateTeleporter( 2088, 223, 19, 225, 604, 0, map, false );
				CreateTeleporter( 2088, 224, 19, 225, 605, 0, map, false );
				
                                //Tunele Okari
                                CreateTeleporter( 268, 1380, 0, 1907, 1564, 0, map, false );
								CreateTeleporter( 269, 1380, 0, 1908, 1564, 0, map, false ); 
                                CreateTeleporter( 270, 1380, 0, 1909, 1564, 0, map, false ); 

                                CreateTeleporter( 1907, 1565, 0, 268, 1381, 0, map, false );
								CreateTeleporter( 1908, 1565, 0, 269, 1381, 0, map, false ); 
                                CreateTeleporter( 1909, 1565, 0, 270, 1381, 0, map, false ); 

                                //Przejscie Gusulia-Okaria
                                CreateTeleporter( 961, 164, 5, 1901, 1559, 0, map, false );
                                CreateTeleporter( 962, 164, 5, 1901, 1558, 0, map, false ); 

                                CreateTeleporter( 1899, 1560, 0, 960, 165, 5, map, false );
                                CreateTeleporter( 1899, 1559, 0, 961, 165, 5, map, false ); 
                                CreateTeleporter( 1899, 1558, 0, 962, 165, 5, map, false ); 

                                //Port Valeria-Gusulia
                                CreateTeleporter( 291, 686, -2, 798, 253, -2, map, false ); 
                                CreateTeleporter( 798, 257, -2, 295, 686, -2, map, false ); 
                                
                                // Dziupla Zlodzei
                                //Wejscie KeyWordTeleporter w deko (slowo = mantikora)
                                //CreateTeleporter( 420, 568, 3, 2036, 612, 17, map, false );
                                
                                CreateTeleporter( 2035, 612, 20, 420, 568, 3, map, false );
                                CreateTeleporter( 2035, 613, 20, 420, 568, 3, map, false );
                                CreateTeleporter( 2036, 613, 20, 420, 568, 3, map, false ); 
                                
                                

				// gniazdo terathan
				CreateTeleporter( 417, 613, 0, 2060, 1564, 25, map, false );
				CreateTeleporter( 418, 613, 0, 2060, 1564, 25, map, false );
				CreateTeleporter( 419, 613, 0, 2061, 1564, 25, map, false );
				CreateTeleporter( 420, 613, 0, 2061, 1564, 25, map, false );
				
				CreateTeleporter( 2060, 1565, 25, 418, 614, 0, map, false );
				CreateTeleporter( 2061, 1565, 25, 419, 614, 0, map, false );

                                CreateTeleporter( 2199, 1526, 25, 2030, 1475, 25, map, false );
				CreateTeleporter( 2200, 1526, 25, 2031, 1475, 25, map, false );
        
         // wyspa prawo od zakonu - jaskinia championa

	CreateTeleporter( 122, 685, 0, 2240, 1426, 25, map, false );
        CreateTeleporter( 123, 685, 0, 2240, 1426, 25, map, false );
        CreateTeleporter( 124, 685, 0, 2241, 1426, 25, map, false );
        CreateTeleporter( 125, 685, 0, 2241, 1426, 25, map, false );

				CreateTeleporter( 2240, 1427, 25, 123, 685, 0, map, false );
				CreateTeleporter( 2241, 1427, 25, 124, 685, 0, map, false );

                                CreateTeleporter( 2252, 1485, 0, 2198, 1216, 2, map, false );

                                CreateTeleporter( 2202, 1216, 2, 2257, 1484, 0, map, false );

				CreateTeleporter( 2030, 1475, 25, 2199, 1528, 25, map, false );
				CreateTeleporter( 2031, 1475, 25, 2200, 1528, 25, map, false );
 
                                //Opuszczone Tunele
                                CreateTeleporter( 471, 734, 0, 2258, 722, 12, map, false );
				CreateTeleporter( 472, 734, 0, 2258, 722, 12, map, false );
				CreateTeleporter( 473, 734, 0, 2259, 722, 12, map, false );
				CreateTeleporter( 474, 734, 0, 2259, 722, 12, map, false );

                                CreateTeleporter( 2258, 723, 17, 471, 735, 0, map, false );
				CreateTeleporter( 2259, 723, 17, 472, 735, 0, map, false );

                                CreateTeleporter( 2191, 697, -20, 2108, 677, 12, map, false );
				CreateTeleporter( 2192, 697, -20, 2109, 677, 12, map, false );
				CreateTeleporter( 2108, 678, 17, 2191, 699, -13, map, false );
				CreateTeleporter( 2109, 678, 17, 2192, 699, -13, map, false );

                                //Podziemia Upadlych
                                CreateTeleporter( 270, 1515, 0, 1697, 93, 12, map, false );
				CreateTeleporter( 271, 1515, 0, 1697, 93, 12, map, false );
				CreateTeleporter( 272, 1515, 0, 1698, 93, 12, map, false );
				CreateTeleporter( 273, 1515, 0, 1698, 93, 12, map, false );

                                CreateTeleporter( 1697, 94, 17, 271, 1517, 0, map, false );
				CreateTeleporter( 1698, 94, 17, 272, 1517, 0, map, false );

                                CreateTeleporter( 1652, 33, -20, 1789, 89, 17, map, false );
				CreateTeleporter( 1652, 34, -20, 1789, 90, 17, map, false );
				CreateTeleporter( 1790, 89, 20, 1654, 33, -13, map, false );
				CreateTeleporter( 1790, 90, 20, 1654, 34, -13, map, false );

                                CreateTeleporter( 1779, 50, -20, 1796, 187, 7, map, false );
				CreateTeleporter( 1779, 51, -20, 1796, 188, 7, map, false );
				CreateTeleporter( 1798, 187, 17, 1781, 50, -13, map, false );
				CreateTeleporter( 1798, 188, 17, 1781, 51, -13, map, false );

                                //Jaskinie Miedzi
                                CreateTeleporter( 843, 179, 5, 2105, 99, 12, map, false );
				CreateTeleporter( 844, 179, 5, 2105, 99, 12, map, false );
				CreateTeleporter( 845, 179, 5, 2106, 99, 12, map, false );
				CreateTeleporter( 846, 179, 5, 2106, 99, 12, map, false );

                                CreateTeleporter( 2105, 100, 17, 844, 180, 5, map, false );
				CreateTeleporter( 2106, 100, 17, 845, 180, 5, map, false ); 

                                CreateTeleporter( 2075, 59, -20, 1939, 37, 17, map, false );
				CreateTeleporter( 2075, 60, -20, 1939, 38, 17, map, false );
				CreateTeleporter( 1940, 37, 20, 2077, 59, -13, map, false );
				CreateTeleporter( 1940, 38, 20, 2077, 60, -13, map, false );

                                CreateTeleporter( 1912, 29, -20, 1942, 149, 14, map, false );
				CreateTeleporter( 1912, 30, -20, 1942, 150, 14, map, false );
				CreateTeleporter( 1943, 149, 19, 1913, 29, -18, map, false );
				CreateTeleporter( 1943, 150, 19, 1913, 30, -18, map, false );

                                //Studnia Umarlych
                                CreateTeleporter( 1194, 1029, 2, 1859, 353, 12, map, false );
				CreateTeleporter( 1194, 1028, 2, 1859, 353, 12, map, false );
				CreateTeleporter( 1194, 1027, 2, 1859, 352, 12, map, false );
				CreateTeleporter( 1194, 1026, 2, 1859, 352, 12, map, false );

                                CreateTeleporter( 1860, 352, 17, 1196, 1027, 2, map, false );
				CreateTeleporter( 1860, 353, 17, 1196, 1028, 2, map, false );

                                CreateTeleporter( 1810, 379, -20, 1969, 408, 12, map, false );
				CreateTeleporter( 1810, 380, -20, 1969, 409, 12, map, false );
				CreateTeleporter( 1970, 408, 17, 1812, 379, -13, map, false );
				CreateTeleporter( 1970, 409, 17, 1812, 380, -13, map, false );

                                CreateTeleporter( 1958, 372, -20, 2095, 387, 12, map, false );
				CreateTeleporter( 1958, 373, -20, 2095, 388, 12, map, false );
				CreateTeleporter( 2096, 387, 17, 1959, 372, -18, map, false );
				CreateTeleporter( 2096, 388, 17, 1959, 373, -18, map, false );

                                CreateTeleporter( 2126, 414, -20, 1706, 497, 17, map, false );
				CreateTeleporter( 2127, 414, -20, 1707, 497, 17, map, false );
				CreateTeleporter( 1706, 498, 20, 2126, 415, -18, map, false );
				CreateTeleporter( 1707, 498, 20, 2127, 415, -18, map, false );
				
				//Ognista Glebia
                                CreateTeleporter( 1090, 1082, 2, 1842, 786, 14, map, false );
				CreateTeleporter( 1090, 1081, 2, 1842, 786, 14, map, false );
				CreateTeleporter( 1090, 1080, 2, 1842, 785, 14, map, false );
				CreateTeleporter( 1090, 1079, 2, 1842, 785, 14, map, false );

                                CreateTeleporter( 1843, 785, 19, 1092, 1080, 2, map, false );
				CreateTeleporter( 1843, 786, 19, 1092, 1081, 2, map, false );

                                CreateTeleporter( 1806, 688, -20, 1826, 845, 7, map, false );
				CreateTeleporter( 1806, 689, -20, 1826, 846, 7, map, false );
				CreateTeleporter( 1828, 845, 17, 1808, 688, -13, map, false );
				CreateTeleporter( 1828, 846, 17, 1808, 689, -13, map, false );
                                
                                //Podziemne Jezioro
                                CreateTeleporter( 1027, 151, 5, 1683, 428, 2, map, false );
				CreateTeleporter( 1028, 151, 5, 1683, 428, 2, map, false );
				CreateTeleporter( 1029, 151, 5, 1684, 428, 2, map, false );
				CreateTeleporter( 1030, 151, 5, 1684, 428, 2, map, false );

                                CreateTeleporter( 1683, 429, 0, 1028, 153, 5, map, false );
				CreateTeleporter( 1684, 429, 0, 1029, 153, 5, map, false );

                                CreateTeleporter( 1689, 373, -20, 1936, 476, 12, map, false );
				CreateTeleporter( 1689, 374, -20, 1936, 477, 12, map, false );
				CreateTeleporter( 1937, 476, 17, 1691, 373, -13, map, false );
				CreateTeleporter( 1937, 477, 17, 1691, 374, -13, map, false );

                                //Rojowisko Arachnidow
                                CreateTeleporter( 1271, 1026, 2, 2214, 51, 14, map, false );
				CreateTeleporter( 1272, 1026, 2, 2214, 51, 14, map, false );
				CreateTeleporter( 1273, 1026, 2, 2215, 51, 14, map, false );
				CreateTeleporter( 1274, 1026, 2, 2215, 51, 14, map, false );

                                CreateTeleporter( 2214, 52, 20, 1272, 1028, 2, map, false );
				CreateTeleporter( 2215, 52, 20, 1273, 1028, 2, map, false );

                                CreateTeleporter( 2230, 63, -20, 1672, 157, 14, map, false );
				CreateTeleporter( 2230, 62, -20, 1672, 156, 14, map, false );
				CreateTeleporter( 1673, 156, 19, 2232, 62, -13, map, false );
				CreateTeleporter( 1673, 157, 19, 2232, 63, -13, map, false );

                                //Jaszczurze Gniazdo
                                CreateTeleporter( 1385, 1459, 1, 1836, 506, 12, map, false );
				CreateTeleporter( 1385, 1458, 1, 1836, 506, 12, map, false );
				CreateTeleporter( 1385, 1457, 1, 1836, 505, 12, map, false );
				CreateTeleporter( 1385, 1456, 1, 1836, 505, 12, map, false );

                                CreateTeleporter( 1837, 505, 17, 1387, 1457, 0, map, false );
				CreateTeleporter( 1837, 506, 17, 1387, 1458, 0, map, false );

                                //Zabia Grota
                                CreateTeleporter( 1327, 985, 2, 2113, 824, 14, map, false );
				CreateTeleporter( 1327, 984, 2, 2113, 824, 14, map, false );
				CreateTeleporter( 1327, 983, 2, 2113, 823, 14, map, false );
				CreateTeleporter( 1327, 982, 2, 2113, 823, 14, map, false );

                                CreateTeleporter( 2114, 823, 19, 1329, 983, 2, map, false );
				CreateTeleporter( 2114, 824, 19, 1329, 984, 2, map, false );

                                //Podziemna Forteca
                                CreateTeleporter( 1386, 528, 5, 2217, 843, 12, map, false );
				CreateTeleporter( 1387, 528, 5, 2217, 843, 12, map, false );
				CreateTeleporter( 1388, 528, 5, 2218, 843, 12, map, false );
				CreateTeleporter( 1389, 528, 5, 2218, 843, 12, map, false );

                                CreateTeleporter( 2218, 844, 17, 1388, 530, 3, map, false );
				CreateTeleporter( 2217, 844, 17, 1387, 530, 3, map, false );

                                //Pustynna jaskinia 1
                                CreateTeleporter( 231, 760, 2, 2251, 995, 0, map, false );
				CreateTeleporter( 232, 760, 2, 2251, 995, 0, map, false );
				CreateTeleporter( 233, 760, 2, 2252, 995, 0, map, false );
				CreateTeleporter( 234, 760, 2, 2252, 995, 0, map, false );

                                CreateTeleporter( 2251, 996, 0, 232, 762, 2, map, false );
				CreateTeleporter( 2252, 996, 0, 233, 762, 2, map, false );

                                //Pustynna jaskinia 2
                                CreateTeleporter( 287, 787, 0, 2020, 1001, 0, map, false );
				CreateTeleporter( 288, 787, 0, 2020, 1001, 0, map, false );
				CreateTeleporter( 289, 787, 0, 2021, 1001, 0, map, false );
				CreateTeleporter( 290, 787, 0, 2021, 1001, 0, map, false );

                                CreateTeleporter( 2021, 1002, 0, 289, 788, 0, map, false );
				CreateTeleporter( 2020, 1002, 0, 288, 788, 0, map, false );

                                //Pustynna jaskinia 3
                                CreateTeleporter( 345, 783, 0, 1913, 981, 0, map, false );
				CreateTeleporter( 345, 782, 0, 1913, 981, 0, map, false );
				CreateTeleporter( 345, 781, 0, 1913, 980, 0, map, false );
				CreateTeleporter( 345, 780, 0, 1913, 980, 0, map, false );

                                CreateTeleporter( 1914, 980, 0, 347, 781, 0, map, false );
				CreateTeleporter( 1914, 981, 0, 347, 782, 0, map, false );

                                //Pustynna jaskinia 4
                                CreateTeleporter( 325, 812, 0, 1686, 986, 0, map, false );
				CreateTeleporter( 326, 812, 0, 1686, 986, 0, map, false );
				CreateTeleporter( 327, 812, 0, 1687, 986, 0, map, false );
				CreateTeleporter( 328, 812, 0, 1687, 986, 0, map, false );

                                CreateTeleporter( 1687, 987, 0, 327, 814, 0, map, false );
				CreateTeleporter( 1686, 987, 0, 326, 814, 0, map, false );

                                //Jaskinia 5
                                CreateTeleporter( 121, 701, 0, 1709, 909, 0, map, false );
				CreateTeleporter( 121, 700, 0, 1709, 908, 0, map, false );
				CreateTeleporter( 121, 699, 0, 1709, 908, 0, map, false );

                                CreateTeleporter( 1710, 909, 0, 122, 701, 0, map, false );
				CreateTeleporter( 1710, 908, 0, 122, 700, 0, map, false );

                                //Pajencza jasknia
                                CreateTeleporter( 753, 146, 5, 1810, 982, 0, map, false );
				CreateTeleporter( 753, 145, 5, 1810, 982, 0, map, false );
				CreateTeleporter( 753, 144, 5, 1810, 981, 0, map, false );
				CreateTeleporter( 753, 143, 5, 1810, 981, 0, map, false );

                                CreateTeleporter( 1811, 981, 0, 754, 144, 5, map, false );
				CreateTeleporter( 1811, 982, 0, 754, 145, 5, map, false );
				
				//Jaskinia 1 wyspa nad zakonem
				CreateTeleporter( 110, 218, 2, 1872, 293, 0, map, false );
				CreateTeleporter( 110, 217, 3, 1872, 292, 0, map, false );
				
				CreateTeleporter( 1872, 293, 0, 110, 218, 2, map, false );
				CreateTeleporter( 1872, 292, 0, 110, 217, 2, map, false );
				
				//jaskinia 2 wyspa nad zakonem
				CreateTeleporter( 102, 177, 2, 1990, 678, 17, map, false );
				CreateTeleporter( 102, 176, 2, 1990, 678, 17, map, false );
				CreateTeleporter( 102, 175, 2, 1990, 679, 17, map, false );
				CreateTeleporter( 102, 174, 2, 1990, 679, 17, map, false );
				
				CreateTeleporter( 1990, 678, 17, 102, 176, 2, map, false );
				CreateTeleporter( 1990, 679, 17, 102, 175, 2, map, false );
				
				CreateTeleporter( 1957, 767, -20, 1970, 855, 17, map, false );
				CreateTeleporter( 1957, 768, -20, 1970, 856, 17, map, false );
				
				CreateTeleporter( 1970, 855, 17, 1957, 768, -20, map, false );
				CreateTeleporter( 1970, 856, 17, 1957, 767, -20, map, false );
				
				//Jaskinia Barna 1 S-----------------ok
                                CreateTeleporter( 721, 206, 5, 1799, 275, 0, map, false );
				CreateTeleporter( 722, 206, 5, 1799, 275, 0, map, false );
				CreateTeleporter( 723, 206, 5, 1800, 275, 0, map, false );
				CreateTeleporter( 724, 206, 5, 1800, 275, 0, map, false );
				
				CreateTeleporter( 1799, 275, 0, 722, 206, 5, map, false );
				CreateTeleporter( 1800, 275, 0, 723, 206, 5, map, false );
				
				//Jaskinia Barna 2 S-----------------ok
                                CreateTeleporter( 758, 187, 5, 2205, 297, 0, map, false );
				CreateTeleporter( 759, 187, 5, 2205, 297, 0, map, false );
				CreateTeleporter( 760, 187, 5, 2206, 297, 0, map, false );
				CreateTeleporter( 761, 187, 5, 2206, 297, 0, map, false );
				
                                CreateTeleporter( 2205, 297, 0, 759, 187, 5, map, false );
                                CreateTeleporter( 2206, 297, 0, 760, 187, 5, map, false );

				
                                //Jaskinia Barna 3 W-----------------ok
                                CreateTeleporter( 785, 162, 5, 1707, 272, 0, map, false );
				CreateTeleporter( 785, 161, 5, 1707, 272, 0, map, false );
				CreateTeleporter( 785, 160, 5, 1707, 271, 0, map, false );
				CreateTeleporter( 785, 159, 5, 1707, 271, 0, map, false );
				
				CreateTeleporter( 1707, 272, 0, 785, 161, 5, map, false );
				CreateTeleporter( 1707, 271, 0, 785, 160, 5, map, false );
				
				//Jaskinia Barna 4 S-----------------ok
                                CreateTeleporter( 784, 208, 5, 1971, 619, 0, map, false );
				CreateTeleporter( 785, 208, 5, 1971, 619, 0, map, false );
				CreateTeleporter( 786, 208, 5, 1972, 619, 0, map, false );
				CreateTeleporter( 787, 208, 5, 1972, 619, 0, map, false );
				
				CreateTeleporter( 1971, 619, 0, 785, 208, 5, map, false );
				CreateTeleporter( 1972, 619, 0, 786, 208, 5, map, false );
				
				//Jaskinia Barna 5 W-----------------ok despise
                                CreateTeleporter( 930, 173, 5, 5521, 674, 27, map, Map.Felucca, false );
				CreateTeleporter( 930, 172, 5, 5521, 673, 27, map, Map.Felucca, false );
				CreateTeleporter( 930, 171, 5, 5521, 672, 27, map, Map.Felucca, false );
				CreateTeleporter( 930, 170, 5, 5521, 672, 27, map, Map.Felucca, false );
				
				//CreateTeleporter( 5521, 674, 27, 930, 173, 5, Map.Felucca, map, false );
				//CreateTeleporter( 5521, 673, 27, 930, 173, 5, Map.Felucca, map, false );
				//CreateTeleporter( 5521, 672, 27, 930, 173, 5, Map.Felucca, map, false );
				
				//Jaskinia Barna 6 S-----------------ok covetous
                                CreateTeleporter( 953, 293, 5, 5455, 1864, 0, map, Map.Felucca, false );
				CreateTeleporter( 954, 293, 5, 5456, 1864, 0, map, Map.Felucca, false );
				CreateTeleporter( 955, 293, 5, 5457, 1864, 0, map, Map.Felucca, false );
				CreateTeleporter( 956, 293, 5, 5457, 1864, 0, map, Map.Felucca, false );
				
				CreateTeleporter( 5455, 1864, 0, 953, 293, 5, Map.Felucca, map, false);
				CreateTeleporter( 5456, 1864, 0, 954, 293, 5, Map.Felucca, map, false);
				CreateTeleporter( 5457, 1864, 0, 955, 293, 5, Map.Felucca, map, false);
				
				//Jaskinia Guisula 1 W-----------------ok
                                CreateTeleporter( 1066, 267, 5, 1966, 307, 0, map, false );
				CreateTeleporter( 1066, 266, 5, 1966, 307, 0, map, false );
				CreateTeleporter( 1066, 265, 5, 1966, 306, 0, map, false );
				CreateTeleporter( 1066, 264, 5, 1966, 306, 0, map, false );
				
				CreateTeleporter( 1966, 307, 0, 1066, 266, 5, map, false );
				CreateTeleporter( 1966, 306, 0, 1066, 265, 5, map, false  );
				
			        //Jaskinia Guisula 2 S-----------------ok covetous
                                CreateTeleporter( 1089, 282, 5, 5392, 1959, 0, map, Map.Felucca, false );
				CreateTeleporter( 1090, 282, 5, 5393, 1959, 1, map, Map.Felucca, false );
				CreateTeleporter( 1091, 282, 5, 5394, 1959, 3, map, Map.Felucca, false );
				CreateTeleporter( 1092, 282, 5, 5394, 1959, 3, map, Map.Felucca, false );
				
				CreateTeleporter( 5392, 1959, 1, 1089, 282, 5, Map.Felucca, map, false );
				CreateTeleporter( 5393, 1959, 1, 1090, 282, 5, Map.Felucca, map, false );
				CreateTeleporter( 5394, 1959, 1, 1091, 282, 5, Map.Felucca, map, false );
				
				//Jaskinia Guisula 3 W-----------------ok
                                CreateTeleporter( 1118, 234, 5, 2154, 580, 0, map, false );
				CreateTeleporter( 1118, 233, 5, 2154, 580, 0, map, false );
				CreateTeleporter( 1118, 232, 5, 2154, 579, 0, map, false );
				CreateTeleporter( 1118, 231, 5, 2154, 579, 0, map, false );
				
				CreateTeleporter( 2154, 580, 0, 1118, 233, 5, map, false );
				CreateTeleporter( 2154, 579, 0, 1118, 232, 5, map, false );
				
				//Jaskinia Guisula 4 S-----------------ok shame
                                CreateTeleporter( 1133, 218, 5, 5394, 127, -2, map, Map.Felucca, false );
				CreateTeleporter( 1134, 218, 5, 5395, 127, -2, map, Map.Felucca, false );
				CreateTeleporter( 1135, 218, 5, 5396, 127, 0, map, Map.Felucca, false );
				CreateTeleporter( 1136, 218, 5, 5396, 127, 0, map, Map.Felucca, false );
				
				CreateTeleporter( 5394, 127, -2, 1133, 218, 5, Map.Felucca, map, false);
				CreateTeleporter( 5395, 127, -2, 1134, 218, 5, Map.Felucca, map, false);
				CreateTeleporter( 5396, 127, 0, 1135, 218, 5, Map.Felucca, map, false);
				
				//Jaskinia dzika wyspa  S-----------------ok destard
				CreateTeleporter( 537, 1118, 3, 5242, 1007, 4, map, Map.Felucca, false );
                                CreateTeleporter( 538, 1118, 3, 5242, 1007, 4, map, Map.Felucca, false );
				CreateTeleporter( 539, 1118, 2, 5243, 1007, 3, map, Map.Felucca, false );
				CreateTeleporter( 540, 1118, 3, 5244, 1007, 4, map, Map.Felucca, false );
								
				CreateTeleporter( 5242, 1007, 4, 538, 1118, 3, Map.Felucca, map, false);
				CreateTeleporter( 5243, 1007, 3, 539, 1118, 2, Map.Felucca, map, false);
				CreateTeleporter( 5244, 1007, 4, 540, 1118, 3, Map.Felucca, map, false);
				
				CreateTeleporter( 5129, 909, -28, 5142, 797, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5130, 909, -28, 5143, 797, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5131, 909, -28, 5144, 797, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5132, 909, -28, 5145, 797, 22, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5142, 797, 22, 5129, 909, -28, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5143, 797, 22, 5130, 909, -28, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5144, 797, 22, 5131, 909, -28, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5145, 797, 22, 5132, 909, -28, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5153, 811, -25, 5133, 987, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5153, 810, -25, 5133, 986, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5153, 809, -25, 5133, 985, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5153, 808, -25, 5133, 984, 22, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5133, 987, 22, 5153, 811, -25, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5133, 986, 22, 5153, 810, -25, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5133, 985, 22, 5153, 809, -25, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5133, 984, 22, 5153, 808, -25, Map.Felucca, Map.Felucca, false);
				
				
				
				//Jaskinia Guisula 6 W-----------------ok
                                CreateTeleporter( 1298, 262, 5, 1881, 595, 0, map, false );
				CreateTeleporter( 1298, 261, 5, 1881, 595, 0, map, false );
				CreateTeleporter( 1298, 260, 5, 1881, 594, 0, map, false );
				CreateTeleporter( 1298, 259, 5, 1881, 594, 0, map, false );
				
				CreateTeleporter( 1881, 595, 0, 1298, 261, 5, map, false );
				CreateTeleporter( 1881, 594, 0, 1298, 260, 5, map, false );
				
				//Jaskinia Guisula 7 W-----------------ok
                                CreateTeleporter( 1331, 385, 5, 1814, 631, 0, map, false );
				CreateTeleporter( 1331, 384, 5, 1814, 631, 0, map, false );
				CreateTeleporter( 1331, 383, 5, 1814, 630, 0, map, false );
				CreateTeleporter( 1331, 382, 5, 1814, 630, 0, map, false );
				
				CreateTeleporter( 1814, 631, 0, 1331, 384, 5, map, false );
				CreateTeleporter( 1814, 630, 0, 1331, 383, 5, map, false );
				
				//Jaskinia Guisula 8 S-----------------ok
                                CreateTeleporter( 1313, 444, 5, 1725, 1208, 1, map, false );
				CreateTeleporter( 1314, 444, 5, 1726, 1208, 1, map, false );
				CreateTeleporter( 1315, 444, 5, 1727, 1208, 1, map, false );
				CreateTeleporter( 1316, 444, 5, 1727, 1208, 1, map, false );
				
				CreateTeleporter( 1725, 1208, 1, 1313, 444, 5, map, false );
				CreateTeleporter( 1726, 1208, 1, 1314, 444, 5, map, false );
				CreateTeleporter( 1727, 1208, 1, 1315, 444, 5, map, false );

                                // Doom - wejscie Azzan
                                CreateTeleporter( 187, 1382, 0, 381, 133, 33, map, Map.Malas, false );
                                CreateTeleporter( 187, 1381, 0, 381, 133, 33, map, Map.Malas, false );
                                CreateTeleporter( 188, 1381, 0, 381, 133, 33, map, Map.Malas, false );
                                CreateTeleporter( 188, 1382, 0, 381, 133, 33, map, Map.Malas, false );

                                CreateTeleporter( 496, 49, 6, 260, 1400, 2, Map.Malas, map, false);
				
				//wyspa srodka  W----------------- despise - ok
                                CreateTeleporter( 884, 852, 2, 5588, 632, 30, map, Map.Felucca, false );
				CreateTeleporter( 884, 851, 2, 5588, 631, 30, map, Map.Felucca, false );
				CreateTeleporter( 884, 850, 2, 5588, 630, 30, map, Map.Felucca, false );
								
				CreateTeleporter( 5588, 632, 30, 884, 852, 2,  Map.Felucca, map, false);
				CreateTeleporter( 5588, 631, 30, 884, 851, 2,  Map.Felucca, map, false);
				CreateTeleporter( 5588, 630, 30, 884, 850, 2,  Map.Felucca, map, false);
				
				CreateTeleporter( 5573, 630, 42, 5503, 571, 51, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5573, 629, 42, 5503, 570, 51, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5573, 628, 42, 5503, 569, 51, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5503, 571, 51, 5573, 630, 42, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5503, 570, 51, 5573, 629, 42, Map.Felucca, Map.Felucca, false); 
				CreateTeleporter( 5503, 569, 51, 5573, 628, 42, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5573, 634, 22, 5522, 674, 32, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5573, 633, 22, 5522, 673, 32, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5573, 632, 22, 5522, 672, 32, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5522, 674, 32, 5573, 634, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5522, 673, 32, 5573, 633, 22, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5522, 672, 32, 5573, 632, 22, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5386, 757, -8, 5410, 860, 57, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5386, 756, -8, 5410, 859, 57, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5386, 755, -8, 5410, 858, 57, Map.Felucca, Map.Felucca, false);
				
				CreateTeleporter( 5410, 860, 57, 5386, 757, -8, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5410, 859, 57, 5386, 756, -8, Map.Felucca, Map.Felucca, false);
				CreateTeleporter( 5410, 858, 57, 5386, 755, -8, Map.Felucca, Map.Felucca, false);
				
				//Jorfa w----------------- wrong - ok
				CreateTeleporter( 1291, 958, 2, 5824, 631, 5, map, Map.Felucca, false); 
				CreateTeleporter( 1292, 958, 2, 5824, 631, 5, map, Map.Felucca, false);
				CreateTeleporter( 1293, 958, 2, 5825, 631, 5, map, Map.Felucca, false);
				CreateTeleporter( 1294, 958, 2, 5825, 631, 5, map, Map.Felucca, false);
				
				CreateTeleporter( 5824, 631, 5, 1292, 958, 2, Map.Felucca, map, false);
				CreateTeleporter( 5824, 631, 5, 1292, 958, 2, Map.Felucca, map, false);
				CreateTeleporter( 5825, 631, 5, 1293, 958, 2, Map.Felucca, map, false);
				CreateTeleporter( 5825, 631, 5, 1293, 958, 2, Map.Felucca, map, false); 
			
	//kopalnia Guisula
        CreateTeleporter( 843, 179, 5, 1987, 1153, 0, map, false );
        CreateTeleporter( 844, 179, 5, 1987, 1153, 0, map, false );
        CreateTeleporter( 845, 179, 5, 1988, 1153, 0, map, false );
        CreateTeleporter( 846, 179, 5, 1988, 1153, 0, map, false );
        
        CreateTeleporter( 1987, 1153, 0, 844, 179, 5, map, false );
        CreateTeleporter( 1988, 1153, 0, 845, 179, 5, map, false );
        
        //jaskinia guisula --- ice
        CreateTeleporter( 930, 170, 5, 5882, 241, 0, map, Map.Felucca, false );
        CreateTeleporter( 930, 171, 5, 5882, 241, 0, map, Map.Felucca, false );
        CreateTeleporter( 930, 172, 5, 5882, 242, 0, map, Map.Felucca, false );
        CreateTeleporter( 930, 173, 5, 5882, 243, 0, map, Map.Felucca, false );
        
        CreateTeleporter( 5882, 241, 0, 930, 172, 5, Map.Felucca, map, false );
        CreateTeleporter( 5882, 242, 0, 930, 173, 5, Map.Felucca, map, false );
        CreateTeleporter( 5882, 243, 1, 930, 173, 5, Map.Felucca, map, false );
        
        CreateTeleporter( 5849, 239, -25, 5831, 323, 34, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5850, 239, -25, 5832, 323, 34, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5851, 239, -25, 5833, 323, 34, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5852, 239, -25, 5834, 323, 34, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5853, 239, -25, 5835, 323, 34, Map.Felucca, Map.Felucca, false);
        
        CreateTeleporter( 5831, 323, 34, 5849, 239, -25, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5832, 323, 34, 5850, 239, -25, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5833, 323, 34, 5851, 239, -25, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5834, 323, 34, 5852, 239, -25, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5835, 323, 34, 5853, 239, -25, Map.Felucca, Map.Felucca, false);
        
        CreateTeleporter( 5705, 147, -45, 5706, 307, 12, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5705, 146, -45, 5706, 306, 12, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5705, 145, -45, 5706, 305, 12, Map.Felucca, Map.Felucca, false);
        
        CreateTeleporter( 5706, 307, 12, 5705, 147, -45, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5706, 306, 12, 5705, 146, -45, Map.Felucca, Map.Felucca, false);
        CreateTeleporter( 5706, 305, 12, 5705, 145, -45, Map.Felucca, Map.Felucca, false);
        
        
        
        //jaskinia guisula 10
        CreateTeleporter( 953, 293, 5, 1693, 634, 0, map, false );
        CreateTeleporter( 954, 293, 5, 1693, 634, 0, map, false );
        CreateTeleporter( 955, 293, 5, 1694, 634, 0, map, false );
        CreateTeleporter( 956, 293, 5, 1694, 634, 0, map, false );
        
        CreateTeleporter( 1693, 634, 0, 954, 293, 5, map, false );
        CreateTeleporter( 1694, 634, 0, 955, 293, 5, map, false );
        
        //jaskinia guisula 11
        CreateTeleporter( 1396, 432, 5, 2237, 168, 17, map, false );
        CreateTeleporter( 1396, 431, 5, 2237, 168, 17, map, false );
        CreateTeleporter( 1396, 430, 5, 2237, 167, 17, map, false );
        CreateTeleporter( 1396, 429, 5, 2237, 167, 17, map, false );
        
        CreateTeleporter( 2237, 168, 17, 1396, 431, 5, map, false );
        CreateTeleporter( 2237, 167, 17, 1396, 430, 5, map, false );
        
        //jaskinia guisula 12
        CreateTeleporter( 1133, 218, 5, 2105, 100, 17, map, false );
        CreateTeleporter( 1134, 218, 5, 2105, 100, 17, map, false );
        CreateTeleporter( 1135, 218, 5, 2106, 100, 17, map, false );
        CreateTeleporter( 1136, 218, 5, 2106, 100, 17, map, false );
        
        CreateTeleporter( 2105, 100, 17, 1134, 218, 5, map, false );
        CreateTeleporter( 2106, 100, 17, 1135, 218, 5, map, false );
        
        //jaskinia guisula 13
        CreateTeleporter( 1201, 178, 5, 2081, 513, 17, map, false );
        CreateTeleporter( 1202, 178, 5, 2081, 513, 17, map, false );
        CreateTeleporter( 1203, 178, 5, 2082, 513, 17, map, false );
        CreateTeleporter( 1204, 178, 5, 2082, 513, 17, map, false );
        
        CreateTeleporter( 2081, 513, 17, 1202, 178, 5, map, false );
        CreateTeleporter( 2082, 513, 17, 1203, 178, 5, map, false );
        
        //jakisnia guisula 14
        CreateTeleporter( 1089, 282, 5, 2099, 1072, 0, map, false );
        CreateTeleporter( 1090, 282, 5, 2099, 1072, 0, map, false );
        CreateTeleporter( 1091, 282, 5, 2100, 1072, 0, map, false );
        CreateTeleporter( 1092, 282, 5, 2100, 1072, 0, map, false );
        
        CreateTeleporter( 2099, 1072, 0, 1090, 282, 5, map, false );
        CreateTeleporter( 2100, 1072, 0, 1091, 282, 5, map, false );
        
        //Jorfa ----------- Deceit
        CreateTeleporter( 1050, 1095, 2, 5186, 639, 0, map, Map.Felucca, false);
        CreateTeleporter( 1051, 1095, 2, 5187, 639, 0, map, Map.Felucca, false);
        CreateTeleporter( 1052, 1095, 2, 5188, 639, 0, map, Map.Felucca, false);
        CreateTeleporter( 1053, 1095, 2, 5189, 639 ,0, map, Map.Felucca, false);
        
        CreateTeleporter( 5186, 639, 0, 1050, 1095, 2, Map.Felucca, map, false);
        CreateTeleporter( 5187, 639, 0, 1051, 1095, 2, Map.Felucca, map, false);
	CreateTeleporter( 5188, 639, 0, 1052, 1095, 2, Map.Felucca, map, false);
	CreateTeleporter( 5189, 639 ,0, 1053, 1095, 2, Map.Felucca, map, false);
	      
	CreateTeleporter( 5216, 586, -13, 5304, 532, 7, Map.Felucca, Map.Felucca, false); 
	CreateTeleporter( 5217, 586, -13, 5305, 532, 7, Map.Felucca, Map.Felucca, false);
	CreateTeleporter( 5218, 586, -13, 5306, 532, 7, Map.Felucca, Map.Felucca, false);
	      
	CreateTeleporter( 5304, 532, 7, 5216, 586, -13, Map.Felucca, Map.Felucca, false);
	CreateTeleporter( 5305, 532, 7, 5217, 586, -13, Map.Felucca, Map.Felucca, false);
	CreateTeleporter( 5306, 532, 7, 5218, 586, -13, Map.Felucca, Map.Felucca, false);
	      
	      	
			}
			
			public void CreateTeleportersMap3( Map map )
			{
			}

			public void CreateTeleportersMap4( Map map )
			{
			}
			public void CreateTeleportersTrammel( Map map )
			{
			}

			public void CreateTeleportersFelucca( Map map )
			{
				// Star room
				CreateTeleporter( 5140, 1773, 0, 5171, 1586, 0, map, false );
			}

			public int CreateTeleporters()
			{
				CreateTeleportersMap( Map.Felucca );
				CreateTeleportersMap( Map.Trammel );
				CreateTeleportersTrammel( Map.Trammel );
				CreateTeleportersFelucca( Map.Felucca );
				CreateTeleportersMap2( Map.Ilshenar );
				CreateTeleportersMap3( Map.Malas );
				CreateTeleportersMap4( Map.Tokuno );
				return m_Count;
			}
		}
	}
}
