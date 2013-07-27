using System;
using System.IO;
using System.Drawing;

namespace Bespoke.Common.LinearAlgebra
{
	/// <summary>
	/// A 2D vector.
	/// </summary>
	public struct Vector2
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> class.
		/// </summary>
        /// <param name="source">The source of the vector.</param>
        public Vector2(Point source)
            : this(source.X, source.Y)
		{
		}

		/// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> class.
		/// </summary>
		/// <param name="x">The x value.</param>
		/// <param name="y">The y value.</param>
		public Vector2(float x, float y)
		{
			X = x;
			Y = y;
		}

		#region Operators

		/// <summary>
		/// Negate a <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value">The <see cref="Vector2"/> to negate.</param>
		/// <returns>The negated vector.</returns>
		public static Vector2 operator -(Vector2 value)
		{
			return new Vector2(-value.X, -value.Y);
		}

		/// <summary>
		/// Add two vectors.
		/// </summary>
		/// <param name="lhs">The first operand.</param>
		/// <param name="rhs">The second operand.</param>
		/// <returns>The computed <see cref="Vector2"/>.</returns>
		public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
		{
			Vector2 vector;
			vector.X = lhs.X + rhs.X;
			vector.Y = lhs.Y + rhs.Y;

			return vector;
		}

        /// <summary>
        /// Subtract two vectors.
        /// </summary>
        /// <param name="lhs">The first operand.</param>
        /// <param name="rhs">The second operand.</param>
        /// <returns>The computed <see cref="Vector2"/>.</returns>
		public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
		{
			Vector2 vector;
			vector.X = lhs.X - rhs.X;
			vector.Y = lhs.Y - rhs.Y;

			return vector;
		}

		/// <summary>
		/// Scale a <see cref="Vector2"/> by a scalar.
		/// </summary>
		/// <param name="scaleFactor">The scale factor.</param>
		/// <param name="value">The <see cref="Vector2"/> to scale.</param>
        /// <returns>The scaled <see cref="Vector2"/>.</returns>
		public static Vector2 operator *(float scaleFactor, Vector2 value)
		{
			return new Vector2(value.X * scaleFactor, value.Y * scaleFactor);
		}

		/// <summary>
		/// Compares a vector for equality with another vector.
		/// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
		/// <returns>true if the vectors are equal; otherwise, false.</returns>
		public static bool operator ==(Vector2 lhs, Vector2 rhs)
		{
			return (lhs.X == rhs.X && lhs.Y == rhs.Y);
		}

        /// <summary>
        /// Compares a vector for inequality with another vector.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>true if the vectors not are equal; otherwise, false.</returns>
		public static bool operator !=(Vector2 lhs, Vector2 rhs)
		{
			return !(lhs == rhs);
		}

		#endregion

		/// <summary>
		/// Compute the dot product of two vectors.
		/// </summary>
        /// <param name="lhs">The first operand.</param>
        /// <param name="rhs">The second operand.</param>
		/// <returns>The dot product of the two vectors.</returns>
		public static float Dot(Vector2 lhs, Vector2 rhs)
		{
			return ((lhs.X * rhs.X) + (lhs.Y * rhs.Y));
		}

        /// <summary>
        /// Compute the distance between two vectors.
        /// </summary>
        /// <param name="lhs">The first vector.</param>
        /// <param name="rhs">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
		public static float Distance(Vector2 lhs, Vector2 rhs)
		{
			float deltaX = lhs.X - rhs.X;
			float deltaY = lhs.Y - rhs.Y;
			float distanceSquared = (deltaX * deltaX) + (deltaY * deltaY);

			return (float)Math.Sqrt((double)distanceSquared);
		}

		/// <summary>
		/// Normalize a vector.
		/// </summary>
		/// <param name="vector">The vector to normalize.</param>
		/// <returns>The normalized vector.</returns>
		public static Vector2 Normalize(Vector2 vector)
		{
            Vector2 normalizedVector = vector;
            normalizedVector.Normalize();
		
			return normalizedVector;
		}

        /// <summary>
        /// Get the angle between two vectors.
        /// </summary>
        /// <param name="fromVector">The first vector.</param>
        /// <param name="toVector">The second vector.</param>
        /// <returns>The angle (in radians) between the two vectors.</returns>
        public static float GetAngle(Vector2 fromVector, Vector2 toVector)
        {
            fromVector.Normalize();
            toVector.Normalize();

            float angle = (float)Math.Acos(Vector2.Dot(fromVector, toVector));

            if (toVector.X - fromVector.X < 0.0f)
            {
                angle *= -1;
            }

            return angle;
        }

		/// <summary>
		/// Deserialize a vector.
		/// </summary>
		/// <param name="reader">The binary reader to read from.</param>
		/// <returns>The deserialized vector.</returns>
		public static Vector2 Load(BinaryReader reader)
		{
			Assert.ParamIsNotNull("reader", reader);

			float x = reader.ReadSingle();
			float y = reader.ReadSingle();

			return new Vector2(x, y);
		}

		/// <summary>
		/// Serialize a vector.
		/// </summary>
		/// <param name="writer">The binary writer to write to.</param>
		public void Save(BinaryWriter writer)
		{
			Assert.ParamIsNotNull("writer", writer);

			writer.Write(X);
			writer.Write(Y);
		}

		/// <summary>
		/// Determines whether the specified System.Object is equal to the Vector.
		/// </summary>
		/// <param name="other">The System.Object to compare with the current Vector.</param>
		/// <returns>true if the specified System.Object is equal to the current Vector; false otherwise.</returns>
		public override bool Equals(object other)
		{
			if (!(other is Vector2))
			{
				return false;
			}

			return this == (Vector2)other;
		}

		/// <summary>
		/// Gets the hash code of this object.
		/// </summary>
		/// <returns>Hash code of this object.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Normalize the vector.
		/// </summary>
		public void Normalize()
		{
			float inverseLength = 1.0f / Length();
			X *= inverseLength;
			Y *= inverseLength;
		}

		/// <summary>
		/// Get the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		public float Length()
		{
			double lengthSquared = (X * X) + (Y * Y);
			return (float)Math.Sqrt(lengthSquared);
		}

        /// <summary>
        /// Convert the vector to a <see cref="Point"/> object.
        /// </summary>
        /// <returns>A <see cref="Point"/> object representing the vector.</returns>
        /// <remarks>Loses floating-point precision.</remarks>
        public Point ToPoint()
        {
            return new Point((int)X, (int)Y);
        }

		/// <summary>
		/// Gets a <see cref="Vector2"/> object with X and Y components.
		/// </summary>
		public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

		/// <summary>
		/// The X vector component.
		/// </summary>
		public float X;

		/// <summary>
        /// The Y vector component.
		/// </summary>
		public float Y;
	}
}
