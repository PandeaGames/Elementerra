using System.Collections.Generic;
using NUnit.Framework;
using Terra;
using Terra.MonoViews;
using Terra.Utils;
using UnityEngine;

namespace Tests
{
    public class ObjectStreamingTests
    {
        private class CompareTerraAreas : Comparer<TerraArea>
        {
            public override int Compare(TerraArea a, TerraArea b)
            {
                if (a.Area == b.Area)
                {
                    if (a.x == b.x)
                    {
                        if(a.y == b.y) return 0;
                        
                        return a.y > b.y ? -1 : 1;
                    }
                    
                    return a.x > b.x ? -1 : 1;
                }
                
                return a.Area > b.Area ? -1 : 1;
            }
        }
        
        private static CompareTerraAreas terraAreasCcomparer = new CompareTerraAreas();
        
        private struct Case
        {
            public TerraVector from;
            public TerraVector to;
            public int r;
            public TerraArea[] expectedAdd;
            public TerraArea[] expectedRemove;
        }

        private Dictionary<string, Case> _cases;

        [SetUp]
        public void Setup()
        {
            _cases = new Dictionary<string, Case>();

            _cases.Add("case_1.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(55, 50),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 60, 5, 20),//right side add
                    new TerraArea(45, 60, 0, 20),//left side add
                    new TerraArea(50, 60, 15, 0),//top side add
                    new TerraArea(50, 40, 15, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(65, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 5, 20),//left side remove
                    new TerraArea(45, 60, 15, 0),//top side remove
                    new TerraArea(45, 40, 15, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_1.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(150, 50),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(140, 60, 20, 20),//right side add
                    new TerraArea(140, 60, 0, 20),//left side add
                    new TerraArea(160, 60, 0, 0),//top side add
                    new TerraArea(160, 40, 0, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(160, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 20, 20),//left side remove
                    new TerraArea(140, 60, 0, 0),//top side remove
                    new TerraArea(140, 40, 0, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_2.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(45, 50),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side add
                    new TerraArea(35, 60, 5, 20),//left side add
                    new TerraArea(35, 60, 15, 0),//top side add
                    new TerraArea(35, 40, 15, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(55, 60, 5, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 15, 0),//top side remove
                    new TerraArea(40, 40, 15, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_2.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(10, 50),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side add
                    new TerraArea(0, 60, 20, 20),//left side add
                    new TerraArea(0, 60, 0, 0),//top side add
                    new TerraArea(0, 40, 0, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(40, 60, 20, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 0, 0),//top side remove
                    new TerraArea(40, 40, 0, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_3.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(50, 55),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 65, 0, 20),//right side add
                    new TerraArea(40, 65, 0, 20),//left side add
                    new TerraArea(40, 65, 20, 5),//top side add
                    new TerraArea(40, 40, 20, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 20, 0),//top side remove
                    new TerraArea(40, 45, 20, 5),//bottom side remove
                }
            });
            
            _cases.Add("case_3.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(50, 150),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 160, 0, 20),//right side add
                    new TerraArea(40, 160, 0, 20),//left side add
                    new TerraArea(40, 160, 20, 20),//top side add
                    new TerraArea(40, 40, 20, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 20, 0),//top side remove
                    new TerraArea(40, 60, 20, 20),//bottom side remove
                }
            });
            
            _cases.Add("case_4.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(50, 45),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 55, 0, 20),//right side add
                    new TerraArea(40, 55, 0, 20),//left side add
                    new TerraArea(40, 55, 20, 0),//top side add
                    new TerraArea(40, 40, 20, 5)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 20, 5),//top side remove
                    new TerraArea(40, 35, 20, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_4.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(50, 10),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 20, 0, 20),//right side add
                    new TerraArea(40, 20, 0, 20),//left side add
                    new TerraArea(40, 20, 20, 0),//top side add
                    new TerraArea(40, 20, 20, 20)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(60, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 20, 20),//top side remove
                    new TerraArea(40, 0, 20, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_5.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(55, 55),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 65, 5, 20),//right side add
                    new TerraArea(45, 65, 0, 20),//left side add
                    new TerraArea(50, 65, 15, 5),//top side add
                    new TerraArea(50, 40, 15, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(65, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 5, 20),//left side remove
                    new TerraArea(45, 60, 15, 0),//top side remove
                    new TerraArea(45, 45, 15, 5),//bottom side remove
                }
            });
            
            _cases.Add("case_5.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(150, 150),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(140, 160, 20, 20),//right side add
                    new TerraArea(140, 160, 0, 20),//left side add
                    new TerraArea(160, 160, 0, 20),//top side add
                    new TerraArea(160, 40, 0, 0)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(160, 60, 0, 20),//right side remove
                    new TerraArea(40, 60, 20, 20),//left side remove
                    new TerraArea(140, 60, 0, 0),//top side remove
                    new TerraArea(140, 60, 0, 20),//bottom side remove
                }
            });
            
            _cases.Add("case_6.0", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(45, 45),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 55, 0, 20),//right side add
                    new TerraArea(35, 55, 5, 20),//left side add
                    new TerraArea(35, 55, 15, 0),//top side add
                    new TerraArea(35, 40, 15, 5)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(55, 60, 5, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 15, 5),//top side remove
                    new TerraArea(40, 35, 15, 0),//bottom side remove
                }
            });
            
            _cases.Add("case_6.1", new Case()
            {
                from = new TerraVector(50, 50),
                to = new TerraVector(10, 10),
                r = 10,
                expectedAdd = new []
                {
                    new TerraArea(60, 20, 0, 20),//right side add
                    new TerraArea(0, 20, 20, 20),//left side add
                    new TerraArea(0, 20, 0, 0),//top side add
                    new TerraArea(0, 20, 0, 20)//bottom side add
                },
                expectedRemove = new []
                {
                    new TerraArea(40, 60, 20, 20),//right side remove
                    new TerraArea(40, 60, 0, 20),//left side remove
                    new TerraArea(40, 60, 0, 20),//top side remove
                    new TerraArea(40, 0, 0, 0),//bottom side remove
                }
            });
        }
        
        [TestCase("case_1.0"), 
         TestCase("case_1.1"),
         TestCase("case_2.0"), 
         TestCase("case_2.1"),
         TestCase("case_3.0"),
         TestCase("case_3.1"),
         TestCase("case_4.0"),
         TestCase("case_4.1"),
         TestCase("case_5.0"),
         TestCase("case_5.1"),
         TestCase("case_6.0"),
         TestCase("case_6.1")]
        public void Stream(string caseKey)
        {
            Case testCase = _cases[caseKey];
            List<TerraArea> addAreas = new List<TerraArea>();
            List<TerraArea> removeAreas = new List<TerraArea>();
            
            TerraAreaUtils.CalculateChangeAreas(
                from:testCase.from,
                to:testCase.to, 
                r:testCase.r,
                addAreas:out addAreas,
                removeAreas:out removeAreas);

            for (int i = 0; i < testCase.expectedAdd.Length; i++)
            {
                Assert.AreEqual(expected:testCase.expectedAdd[i], actual:addAreas[i]);
            }
            
            for (int i = 0; i < testCase.expectedRemove.Length; i++)
            {
                Assert.AreEqual(expected:testCase.expectedRemove[i], actual:removeAreas[i]);
            }
        }
    }
}