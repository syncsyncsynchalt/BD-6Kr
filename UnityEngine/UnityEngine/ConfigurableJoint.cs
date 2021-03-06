using System;

using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class ConfigurableJoint : Joint
	{
		public Vector3 secondaryAxis
		{
			get
			{
				INTERNAL_get_secondaryAxis(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_secondaryAxis(ref value);
			}
		}

		public ConfigurableJointMotion xMotion
		{
			get;
			set;
		}

		public ConfigurableJointMotion yMotion
		{
			get;
			set;
		}

		public ConfigurableJointMotion zMotion
		{
			get;
			set;
		}

		public ConfigurableJointMotion angularXMotion
		{
			get;
			set;
		}

		public ConfigurableJointMotion angularYMotion
		{
			get;
			set;
		}

		public ConfigurableJointMotion angularZMotion
		{
			get;
			set;
		}

		public SoftJointLimitSpring linearLimitSpring
		{
			get
			{
				INTERNAL_get_linearLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_linearLimitSpring(ref value);
			}
		}

		public SoftJointLimitSpring angularXLimitSpring
		{
			get
			{
				INTERNAL_get_angularXLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_angularXLimitSpring(ref value);
			}
		}

		public SoftJointLimitSpring angularYZLimitSpring
		{
			get
			{
				INTERNAL_get_angularYZLimitSpring(out SoftJointLimitSpring value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYZLimitSpring(ref value);
			}
		}

		public SoftJointLimit linearLimit
		{
			get
			{
				INTERNAL_get_linearLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_linearLimit(ref value);
			}
		}

		public SoftJointLimit lowAngularXLimit
		{
			get
			{
				INTERNAL_get_lowAngularXLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_lowAngularXLimit(ref value);
			}
		}

		public SoftJointLimit highAngularXLimit
		{
			get
			{
				INTERNAL_get_highAngularXLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_highAngularXLimit(ref value);
			}
		}

		public SoftJointLimit angularYLimit
		{
			get
			{
				INTERNAL_get_angularYLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYLimit(ref value);
			}
		}

		public SoftJointLimit angularZLimit
		{
			get
			{
				INTERNAL_get_angularZLimit(out SoftJointLimit value);
				return value;
			}
			set
			{
				INTERNAL_set_angularZLimit(ref value);
			}
		}

		public Vector3 targetPosition
		{
			get
			{
				INTERNAL_get_targetPosition(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetPosition(ref value);
			}
		}

		public Vector3 targetVelocity
		{
			get
			{
				INTERNAL_get_targetVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetVelocity(ref value);
			}
		}

		public JointDrive xDrive
		{
			get
			{
				INTERNAL_get_xDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_xDrive(ref value);
			}
		}

		public JointDrive yDrive
		{
			get
			{
				INTERNAL_get_yDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_yDrive(ref value);
			}
		}

		public JointDrive zDrive
		{
			get
			{
				INTERNAL_get_zDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_zDrive(ref value);
			}
		}

		public Quaternion targetRotation
		{
			get
			{
				INTERNAL_get_targetRotation(out Quaternion value);
				return value;
			}
			set
			{
				INTERNAL_set_targetRotation(ref value);
			}
		}

		public Vector3 targetAngularVelocity
		{
			get
			{
				INTERNAL_get_targetAngularVelocity(out Vector3 value);
				return value;
			}
			set
			{
				INTERNAL_set_targetAngularVelocity(ref value);
			}
		}

		public RotationDriveMode rotationDriveMode
		{
			get;
			set;
		}

		public JointDrive angularXDrive
		{
			get
			{
				INTERNAL_get_angularXDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_angularXDrive(ref value);
			}
		}

		public JointDrive angularYZDrive
		{
			get
			{
				INTERNAL_get_angularYZDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_angularYZDrive(ref value);
			}
		}

		public JointDrive slerpDrive
		{
			get
			{
				INTERNAL_get_slerpDrive(out JointDrive value);
				return value;
			}
			set
			{
				INTERNAL_set_slerpDrive(ref value);
			}
		}

		public JointProjectionMode projectionMode
		{
			get;
			set;
		}

		public float projectionDistance
		{
			get;
			set;
		}

		public float projectionAngle
		{
			get;
			set;
		}

		public bool configuredInWorldSpace
		{
			get;
			set;
		}

		public bool swapBodies
		{
			get;
			set;
		}

		private void INTERNAL_get_secondaryAxis(out Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_secondaryAxis(ref Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_linearLimitSpring(out SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_linearLimitSpring(ref SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularXLimitSpring(out SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularXLimitSpring(ref SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularYZLimitSpring(out SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularYZLimitSpring(ref SoftJointLimitSpring value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_linearLimit(out SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_linearLimit(ref SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_lowAngularXLimit(out SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_lowAngularXLimit(ref SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_highAngularXLimit(out SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_highAngularXLimit(ref SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularYLimit(out SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularYLimit(ref SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularZLimit(out SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularZLimit(ref SoftJointLimit value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_targetPosition(out Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_targetPosition(ref Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_targetVelocity(out Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_targetVelocity(ref Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_xDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_xDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_yDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_yDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_zDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_zDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_targetRotation(out Quaternion value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_targetRotation(ref Quaternion value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_targetAngularVelocity(out Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_targetAngularVelocity(ref Vector3 value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularXDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularXDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_angularYZDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_angularYZDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_get_slerpDrive(out JointDrive value) { throw new NotImplementedException("なにこれ"); }

		private void INTERNAL_set_slerpDrive(ref JointDrive value) { throw new NotImplementedException("なにこれ"); }
	}
}
