using Common.Enum;
using LT.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle.Utils
{
	public class BattleDefines
	{
		public const float DECIDE_BTN_NOTIFIED_TIME = 0.3f;

		public const float FIELDCAMERA_DEFAULT_SHIP_DISTANCE = 30f;

		public const float PREFAB_GENERATION_TIME = 0.1f;

		public const float DELAY_DISCARD_TIME = 0.15f;

		public const float FORMATION_BRIGHT_POINT_INTERVAL = 10f;

		public const float FORMATION_INTERVAL_3DVIEW = 1f;

		public const float FORMATION_ADVANCED_OFFS = 5f;

		public const float FIELDCAMERA_DEFAULT_HEIGHT = 4f;

		public const float FIELDDIMCAM_DIM_AMOUNT = 0.75f;

		public const int CENTERLINE_SLOTITEM_MAX_NUM = 3;

		public const float BOSSINSERT_START_BOSS_SCALE = 0.95f;

		public const float FLEET_ADVENT_ROTATE_SPD = -10f;

		public const float FLEET_ADVENT_CLOSEUP_DIST = 30f;

		public const float FLEET_ADVENT_CLOUD_POSY = 20f;

		public const float FLEET_ADVENT_CLOSEUP_TIME = 1f;

		public const float FLEET_ADVENT_CLOSEUP_RATE = 0.95f;

		public const float DETECTION_RISE_CAM_HEIGHT = 50f;

		public const float DETECTION_RISE_CAM_MOVE_Z_RATE = 6f;

		public const float DETECTION_RISE_CAM_TIME = 1.95f;

		public const LeanTweenType DETECTION_RISE_CAM_EASING = LeanTweenType.easeInOutCubic;

		public const float DETECTION_RISE_CAM_CLOUD_OCCURS_HEIGHT_RATE = 0.15f;

		public const int DETECTION_CUTIN_DRAW_AIRCRAFT_MAX = 3;

		public const float DETECTION_ENEMY_FLEET_FOCUS_MOVE_TIME = 2.7f;

		public const float DETECTION_ENEMY_FLEET_FOCUS_MOVE_DISTANCE_RATE = 0.3f;

		public const float DETECTION_ENEMY_FLEET_FOCUS_DISTANCE_CLOSEUP_RATE = 0.7f;

		public const float COMMAND_FORMATION_INVERVAL_3DVIEW = 4f;

		public const int MAX_AIRCRAFT_COUNT = 3;

		public const float SHELLING_ATTACK_CAMERA_TO_TARGET_DICTANCE = 50f;

		public const float SHELLING_ATTACK_CAMERA_CLOSEUP_DISTANCE = 10f;

		public const float SHELLING_ATTACK_PLAY_SLOT_ANIM_DELAY_TIME = 0.033f;

		public const float SHELLING_ATTACK_CLOSE_UP_RATE = 0.98f;

		public const float SHELLING_ATTACK_MOTIONBLUR_AMOUNT = 0.65f;

		public const float SHELLING_ATTACK_DAMATE_EXPLODE_POS_RATE = 0.9f;

		public const float SHELLING_ATTACK_ATTACKER_CLOSEUP_SECOND_TIME_AIRCRAFT = 1.2f;

		public const float SHELLING_ATTACK_PLAY_SHIP_SHELLING_ANIM_DELAY_TIME = 0.4f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_TIME = 0.666f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_MOVE_TIME = 1.1655f;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_POS_RATE = 0.2f;

		public const LeanTweenType SHELLING_ATTACK_CAMERA_ROTATE_LOOK_EASING_TYPE = LeanTweenType.easeInQuad;

		public const float SHELLING_ATTACK_CAMERA_ROTATE_DELAY_TIME = 0.3f;

		public const float SHELLING_PLAY_SHIP_DEFENDER_ANIM_DELAY_TIME = 0.5f;

		public const float SHELLING_ATTACK_PROTECT_FROM_SECOND_CLOSEUP_TIME = 0.425f;

		public const float SHELLING_ATTACK_PROTECT_PROTECTOR_POS_RATE = 0.58f;

		public const float SHELLING_DEPTH_CHARGE_PLAY_DEPTH_CHARGE_DELAY_TIME = 0f;

		public const float TORPEDO_STRAIGHT_DURATION = 2.65f;

		public const float DEATHCRY_NEXTPHASE_DELAY_TIME = 1f;

		public const float RESULT_VETERANSREPORT_FLEET_NUM = 2f;

		public const float RESULT_VETERANSREPORT_BANNER_SLOTIN_INTERVAL_TIME = 0.05f;

		public const float RESULT_VETERANSREPORT_BANNER_INFOIN_INTERVAL_TIME = 0.05f;

		public const float RESULT_VETERANSREPORT_SLOTIN_ALPHA_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_WARVETERANS_GAUGE_DRAW_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_WARVETERANS_GAUGE_UPDATE_TIME = 1f;

		public const float RESULT_VETERANSREPORT_BONUS_AND_MVPSHIP_SHOW_TIME = 0.5f;

		public const float RESULT_VETERANSREPORT_EXP_UPDATE_TIME = 2f;

		public const string FLAGSHIP_WRECK_DECK_FLAGSHIP_MESSAGE = "『{0}』艦隊<br>旗艦「{1}」が<br>大破しました。";

		public const string FLAGSHIP_WRECK_FLEET_HOMING_MESSAGE = "進撃は困難です……帰投します。";

		public const float FLAGSHIP_WRECK_MESSAGE_INTERVAL = 0.05f;

		public const int DAMAGE_CUTIN_DRAW_SHIP_MAX = 3;

		public static readonly Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>> FORMATION_POSITION;

		public static readonly Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>> FORMATION_COMBINEDFLEET_POSITION;

		public static readonly List<Vector3> BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE;

		public static readonly List<Vector3> FLEET_ADVENT_START_CAM_POS;

		public static readonly LeanTweenType FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE;

		public static readonly Dictionary<DetectionProductionType, List<float>> DETECTION_RESULT_LABEL_POS;

		public static readonly Vector3[] AERIAL_BOMB_CAM_POSITION;

		public static readonly Quaternion[] AERIAL_BOMB_CAM_ROTATION;

		public static readonly Vector3[] AERIAL_BOMB_TRANS_ANGLE;

		public static readonly Vector4[] AERIAL_TORPEDO_WAVESPEED;

		public static readonly Vector3[] AERIAL_TORPEDO_CAM_POSITION;

		public static readonly Quaternion[] AERIAL_TORPEDO_CAM_ROTATION;

		public static readonly Vector3[] AERIAL_FRIEND_TORPEDO_POS;

		public static readonly Vector3[] AERIAL_ENEMY_TORPEDO_POS;

		public static readonly List<Vector3> SHELLING_FORMATION_JUDGE_RESULTLABEL_POS;

		public static readonly List<float> SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME;

		public static readonly List<LeanTweenType> SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE;

		public static readonly List<float> SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME;

		public static readonly List<LeanTweenType> SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE;

		public static readonly List<float> SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE;

		public static readonly List<string> RESULT_WINRUNK_JUDGE_TEXT;

		public static SoundKeep SOUND_KEEP;

		public static readonly Dictionary<int, List<Vector3>> DAMAGE_CUT_IN_SHIP_DRAW_POS;

		static BattleDefines()
		{
			BATTLESHIP_TORPEDOSALVO_WAKE_ANGLE = new List<Vector3>
			{
				Vector3.up * -71f,
				Vector3.up * 63f
			};
			FLEET_ADVENT_START_CAM_POS = new List<Vector3>
			{
				new Vector3(-56f, 47.5f, 6.5f),
				new Vector3(40f, 30f, -30f)
			};
			FLEET_ADVENT_FLEET_CLOSEUP_EASEING_TYPE = LeanTweenType.linear;
			DETECTION_RESULT_LABEL_POS = new Dictionary<DetectionProductionType, List<float>>
			{
				{
					DetectionProductionType.Succes,
					new List<float>
					{
						-250f,
						-150f,
						-50f,
						100f,
						200f,
						275f,
						9999f
					}
				},
				{
					DetectionProductionType.SuccesLost,
					new List<float>
					{
						-250f,
						-150f,
						-50f,
						100f,
						200f,
						275f,
						9999f
					}
				},
				{
					DetectionProductionType.Lost,
					new List<float>
					{
						-250f,
						-150f,
						-50f,
						50f,
						150f,
						250f,
						350f
					}
				},
				{
					DetectionProductionType.NotFound,
					new List<float>
					{
						-165f,
						-65f,
						65f,
						165f,
						250f,
						9999f,
						9999f
					}
				}
			};
			AERIAL_BOMB_CAM_POSITION = new Vector3[2]
			{
				new Vector3(20f, 15f, 0f),
				new Vector3(20f, 15f, 0f)
			};
			AERIAL_BOMB_CAM_ROTATION = new Quaternion[2]
			{
				Quaternion.Euler(new Vector3(-16f, 90f, 0f)),
				Quaternion.Euler(new Vector3(-16f, -90f, 0f))
			};
			AERIAL_BOMB_TRANS_ANGLE = new Vector3[2]
			{
				new Vector3(0f, 0f, 0f),
				new Vector3(0f, -180f, 0f)
			};
			AERIAL_TORPEDO_WAVESPEED = new Vector4[2]
			{
				new Vector4(-4f, -2000f, 5f, -1600f),
				new Vector4(-4f, 2000f, 5f, 1600f)
			};
			AERIAL_TORPEDO_CAM_POSITION = new Vector3[2]
			{
				new Vector3(-21.3f, 6.2f, -7f),
				new Vector3(-23f, 6.2f, 5.5f)
			};
			AERIAL_TORPEDO_CAM_ROTATION = new Quaternion[2]
			{
				Quaternion.Euler(new Vector3(16.29f, 90f, 0f)),
				Quaternion.Euler(new Vector3(16f, 90f, 0f))
			};
			AERIAL_FRIEND_TORPEDO_POS = new Vector3[3]
			{
				new Vector3(-9f, 0f, -2.5f),
				new Vector3(0f, 0f, -5f),
				new Vector3(-9f, 0f, -7f)
			};
			AERIAL_ENEMY_TORPEDO_POS = new Vector3[3]
			{
				new Vector3(-11f, 0f, 1f),
				new Vector3(-2f, 0f, -1f),
				new Vector3(-11f, 0f, -2.5f)
			};
			SHELLING_FORMATION_JUDGE_RESULTLABEL_POS = new List<Vector3>
			{
				Vector3.up * 30f,
				Vector3.down * 90f
			};
			SHELLING_ATTACK_ATTACKER_CLOSEUP_TIME = new List<float>
			{
				0.334f,
				1f
			};
			SHELLING_ATTACK_ATTACKER_CLOSEUP_EASING_TYPE = new List<LeanTweenType>
			{
				LeanTweenType.easeInSine,
				LeanTweenType.linear
			};
			SHELLING_ATTACK_DEFENDER_CLOSEUP_TIME = new List<float>
			{
				0.3f,
				1.3f,
				3.504f
			};
			SHELLING_ATTACK_DEFENDER_CLOSEUP_EASING_TYPE = new List<LeanTweenType>
			{
				LeanTweenType.linear,
				LeanTweenType.linear
			};
			SHELLING_ATTACK_PROTECT_CLOSE_UP_RATE = new List<float>
			{
				0.85f,
				0.845f
			};
			RESULT_WINRUNK_JUDGE_TEXT = new List<string>
			{
				string.Empty,
				"敗北",
				"敗北",
				"戦術的敗北!!",
				"戦術的勝利!!",
				"勝利!!",
				"勝利!!",
				"完全勝利!!"
			};
			SOUND_KEEP = default(SoundKeep);
			DAMAGE_CUT_IN_SHIP_DRAW_POS = new Dictionary<int, List<Vector3>>
			{
				{
					1,
					new List<Vector3>
					{
						Vector3.zero,
						Vector3.right * 2000f,
						Vector3.right * 2000f
					}
				},
				{
					2,
					new List<Vector3>
					{
						new Vector3(-200f, 0f, 0f),
						Vector3.right * 2000f,
						new Vector3(200f, 0f, 0f)
					}
				},
				{
					3,
					new List<Vector3>
					{
						new Vector3(-300f, 0f, 0f),
						new Vector3(300f, 0f, 0f),
						Vector3.zero
					}
				}
			};
			FORMATION_POSITION = new Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>>();
			FORMATION_COMBINEDFLEET_POSITION = new Dictionary<BattleFormationKinds1, Dictionary<int, Vector3[]>>();
			CalcFormationBrightPoint();
		}

		private static void CalcFormationBrightPoint()
		{
			float num = 10f;
			IEnumerator enumerator = Enum.GetValues(typeof(BattleFormationKinds1)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					switch ((int)enumerator.Current)
					{
					case 1:
					{
						Dictionary<int, Vector3[]> dictionary2 = new Dictionary<int, Vector3[]>();
						for (int k = 1; k <= 6; k++)
						{
							Vector3[] value = Util.CalcNWayPosZ(Vector3.zero, k, num);
							dictionary2.Add(k, value);
						}
						FORMATION_POSITION.Add(BattleFormationKinds1.TanJuu, dictionary2);
						break;
					}
					case 2:
					{
						Dictionary<int, Vector3[]> dictionary5 = new Dictionary<int, Vector3[]>();
						for (int num6 = 1; num6 <= 6; num6++)
						{
							if (num6 <= 2)
							{
								Vector3[] value2 = Util.CalcNWayPosX(Vector3.zero, num6, num);
								dictionary5.Add(num6, value2);
							}
							else if (num6 <= 4)
							{
								Vector3[] array11 = Util.CalcNWayPosX(Vector3.zero, 2, num);
								Vector3[] value2 = new Vector3[num6];
								for (int num7 = 0; num7 < value2.Length; num7++)
								{
									float num8 = (num7 >= 2) ? num : (0f - num);
									float z = (num7 % 2 != 0) ? array11[1].x : array11[0].x;
									value2[num7].x = num8 / 2f;
									value2[num7].y = array11[0].y;
									value2[num7].z = z;
								}
								dictionary5.Add(num6, value2);
							}
							else
							{
								Vector3[] array12 = Util.CalcNWayPosX(Vector3.zero, 3, num);
								Vector3[] value2 = new Vector3[num6];
								for (int num9 = 0; num9 < value2.Length; num9++)
								{
									float num10 = (num9 >= 3) ? num : (0f - num);
									value2[num9].x = num10 / 2f;
									value2[num9].y = array12[0].y;
									value2[num9].z = array12[num9 % 3].x;
								}
								dictionary5.Add(num6, value2);
							}
						}
						FORMATION_POSITION.Add(BattleFormationKinds1.FukuJuu, dictionary5);
						break;
					}
					case 3:
					{
						Dictionary<int, Vector3[]> dictionary3 = new Dictionary<int, Vector3[]>();
						for (int l = 1; l <= 6; l++)
						{
							Vector3[] array2;
							if (l <= 2)
							{
								array2 = Util.CalcNWayPosZ(Vector3.zero, l, num);
							}
							else if (l <= 5)
							{
								int verNum = (l != 3) ? (l - 1) : l;
								Vector2[] array3 = Mathe.RegularPolygonVertices(verNum, num / 2f, -90f);
								array2 = new Vector3[l];
								if (l == 3)
								{
									int num2 = 0;
									Vector2[] array4 = array3;
									for (int m = 0; m < array4.Length; m++)
									{
										Vector2 vector3 = array4[m];
										array2[num2].x = array3[num2].x;
										array2[num2].y = 0f;
										array2[num2].z = array3[num2].y;
										num2++;
									}
								}
								else
								{
									array2[0] = Vector3.zero;
									int num2 = 0;
									Vector2[] array5 = array3;
									for (int n = 0; n < array5.Length; n++)
									{
										Vector2 vector4 = array5[n];
										array2[num2 + 1].x = array3[num2].x;
										array2[num2 + 1].y = 0f;
										array2[num2 + 1].z = array3[num2].y;
										num2++;
									}
								}
							}
							else
							{
								Vector3[] array6 = Util.CalcNWayPosZ(Vector3.zero, 4, num);
								Vector3[] array7 = Util.CalcNWayPosX(Vector3.zero, 3, num);
								array2 = new Vector3[l];
								array2[0] = array6[1];
								array2[1] = array6[2];
								array2[2] = array6[3];
								array2[3] = array6[0];
								array2[4] = array7[0];
								array2[5] = array7[2];
							}
							dictionary3.Add(l, array2);
						}
						FORMATION_POSITION.Add(BattleFormationKinds1.Rinkei, dictionary3);
						break;
					}
					case 4:
					{
						Dictionary<int, Vector3[]> dictionary4 = new Dictionary<int, Vector3[]>();
						for (int num3 = 1; num3 <= 6; num3++)
						{
							Vector3[] array8 = Util.CalcNWayPosX(Vector3.zero, num3, num / 2f);
							Vector3[] array9 = new Vector3[array8.Length];
							int num4 = 0;
							Vector3[] array10 = array8;
							for (int num5 = 0; num5 < array10.Length; num5++)
							{
								Vector3 vector2 = array10[num5];
								array9[num4].x = vector2.x;
								array9[num4].y = vector2.y;
								array9[num4].z = vector2.x;
								num4++;
							}
							dictionary4.Add(num3, array9);
						}
						FORMATION_POSITION.Add(BattleFormationKinds1.Teikei, dictionary4);
						break;
					}
					case 5:
					{
						Dictionary<int, Vector3[]> dictionary = new Dictionary<int, Vector3[]>();
						for (int i = 1; i <= 6; i++)
						{
							Vector3[] array = Util.CalcNWayPosX(Vector3.zero, i, num);
							for (int j = 0; j < array.Length; j++)
							{
								Vector3 vector = array[j];
								array[j].x = 0f - vector.x;
								array[j].y = vector.y;
								array[j].z = vector.z;
							}
							dictionary.Add(i, array);
						}
						FORMATION_POSITION.Add(BattleFormationKinds1.TanOu, dictionary);
						break;
					}
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
