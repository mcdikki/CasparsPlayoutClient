using System;
using System.Collections.Generic;
using System.Text;

namespace Bespoke.Common.LinearAlgebra
{
    /// <summary>
    /// Defines a matrix.
    /// </summary>
	public class Matrix
	{
		#region Properties

		/// <summary>
		/// Gets a 4x4 identity matrix.
		/// </summary>
		public static Matrix Identity
		{
			get
			{
				return sIdentity;
			}
		}

		/// <summary>
		/// Gets or sets an entry in the matrix.
		/// </summary>
		/// <param name="i">The row to get/set.</param>
		/// <param name="j">The column to get/set.</param>
		/// <returns></returns>
		public double this[int i, int j]
		{
			get
			{
				return this.Data[i, j];
			}
			set
			{
				this.Data[i, j] = value;
			}
		}

		/// <summary>
		/// Indicates whether the matrix is square.
		/// </summary>
		public bool IsSquare
		{
			get
			{
				return (this.Data.GetLength(0) == this.Data.GetLength(1));
			}
		}

		/// <summary>
		/// Indicates whether the matrix is singular.
		/// </summary>
		public bool IsSingular
		{
			get
			{
				return (this.Det() == 0.0);
			}
		}

		/// <summary>
		/// Gets the number of rows in the matrix.
		/// </summary>
		public int rows
		{
			get
			{
				return this.Data.GetLength(0);
			}
		}

		/// <summary>
		/// Gets the number of columns in the matrix.
		/// </summary>
		public int cols
		{
			get
			{
				return this.Data.GetLength(1);
			}
		}

		#endregion

		/// <summary>
		/// Static constructor.
		/// </summary>
		static Matrix()
		{
			double[,] data = { {1.0, 0.0, 0.0, 0.0},
							   {0.0, 1.0, 0.0, 0.0},
				               {0.0, 0.0, 1.0, 0.0},
				               {0.0, 0.0, 0.0, 1.0}
							 };
				              

			sIdentity = new Matrix(data);
		}

		/// <summary>
		/// Creates a new instance of Matrix.
		/// </summary>
		/// <param name="size">The number of rows and columns for the matrix.</param>
		/// <remarks>Creates a square matrix of <paramref name="size"/>.</remarks>
		public Matrix(int size)
		{
			this.Data = new double[size,size];			
		}

		/// <summary>
		/// Creates a new instance of Matrix.
		/// </summary>
		/// <param name="rows">The number of rows.</param>
		/// <param name="cols">The number of columns.</param>
		public Matrix(int rows,int cols)
		{
			this.Data = new double[rows,cols];			
		}

		/// <summary>
		/// Creates a new instance of Matrix.
		/// </summary>
		/// <param name="data">An existing matrix.</param>
		public Matrix(double[,] data)
		{			
			Data = data;
		}

		#region Operators

		/// <summary>
		/// Adds a matrix to another matrix.
		/// </summary>
		/// <param name="M1">Source matrix.</param>
		/// <param name="M2">Source matrix.</param>
		/// <returns>Resulting matrix.</returns>
		public static Matrix operator+ (Matrix M1, Matrix M2)
		{
			int r1 = M1.Data.GetLength(0);int r2 = M2.Data.GetLength(0);  
			int c1 = M1.Data.GetLength(1);int c2 = M2.Data.GetLength(1);  
			if ((r1!=r2)||(c1!=c2))
			{
				throw new System.Exception("Matrix dimensions do not agree");  
			}
			double[,] res = new double[r1,c1]; 
			for (int i=0;i<r1;i++)
			{
				for (int j=0;j<c1;j++)
				{
					res[i,j]=M1.Data[i,j]+M2.Data[i,j];				
				}		
			}

			return new Matrix(res);	
		}

		/// <summary>
		/// Subtracts a matrix from another matrix.
		/// </summary>
		/// <param name="M1">Source matrix.</param>
		/// <param name="M2">Source matrix.</param>
		/// <returns>Resulting matrix.</returns>
		public static Matrix operator- (Matrix M1, Matrix M2)
		{
			int r1 = M1.Data.GetLength(0);int r2 = M2.Data.GetLength(0);  
			int c1 = M1.Data.GetLength(1);int c2 = M2.Data.GetLength(1);  
			if ((r1!=r2)||(c1!=c2))
			{
				throw new System.Exception("Matrix dimensions do not agree");  
			}
			double[,] res = new double[r1,c1]; 
			for (int i=0;i<r1;i++)
			{
				for (int j=0;j<c1;j++)
				{
					res[i,j]=M1.Data[i,j]-M2.Data[i,j];				
				}		
			}

			return new Matrix(res);	
		}

		/// <summary>
		/// Multiplies a matrix by another matrix.
		/// </summary>
		/// <param name="M1">Source matrix.</param>
		/// <param name="M2">Source matrix.</param>
		/// <returns>Resulting matrix.</returns>
		public static Matrix operator* (Matrix M1, Matrix M2)
		{
			int r1 = M1.Data.GetLength(0);int r2 = M2.Data.GetLength(0);  
			int c1 = M1.Data.GetLength(1);int c2 = M2.Data.GetLength(1);  
			if (c1!=r2)
			{
				throw new System.Exception("Matrix dimensions donot agree");  
			}
			double[,] res = new double[r1,c2]; 
			for (int i=0;i<r1;i++)
			{
				for(int j=0;j<c2;j++)
				{
					for(int k=0;k<r2;k++)						
					{
						res[i,j]=  res[i,j] + (M1.Data[i,k]*M2.Data[k,j]);
					}
				}			
			}

			return new Matrix(res);				
		}

		/// <summary>
		/// Divides the components of a matrix by a scalar.
		/// </summary>
		/// <param name="divisor">The divisor.</param>
		/// <param name="M">Source matrix</param>
		/// <returns></returns>
		public static Matrix operator/ (double divisor, Matrix M)
		{
			return new Matrix(scalmul(divisor,INV(M.Data)));		
		}
		
		/// <summary>
		/// Compares a matrix for equality with another matrix.
		/// </summary>
		/// <param name="M1">Source matrix.</param>
		/// <param name="M2">Source matrix.</param>
		/// <returns>true if the matrices are equal; false otherwise.</returns>
		public static bool operator== (Matrix M1, Matrix M2)
		{
			bool B=true;
			int r1 = M1.Data.GetLength(0);int r2 = M2.Data.GetLength(0);  
			int c1 = M1.Data.GetLength(1);int c2 = M2.Data.GetLength(1);  
			if ((r1!=r2)||(c1!=c2))
			{
				return false;
			}
			else
			{
				for (int i=0;i<r1;i++)
				{
					for (int j=0;j<c1;j++)
					{
						if(M1.Data[i,j]!=M2.Data[i,j])
							B=false;
					}		
				}		
			}
			return B;
		}

		/// <summary>
		/// Tests a matrix for inequality with another matrix.
		/// </summary>
		/// <param name="M1">The matrix on the left of the equal sign.</param>
		/// <param name="M2">The matrix on the right of the equal sign.</param>
		/// <returns>true if the matrices are not equal; false otherwise.</returns>
		public static bool operator!= (Matrix M1, Matrix M2)
		{
			return !(M1==M2);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Determines whether the specified System.Object is equal to the Matrix.
		/// </summary>
		/// <param name="other">The System.Object to compare with the current Matrix.</param>
		/// <returns>true if the specified System.Object is equal to the current Matrix; false otherwise.</returns>
		public override bool Equals(object other)
		{
			if (!(other is Matrix))
			{
				return false;
			}

			return this == (Matrix)other;
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
		/// Output the matrix to System.Console.
		/// </summary>
		public void Display()
		{
			int r1 = this.Data.GetLength(0);int c1 = this.Data.GetLength(1);
			for (int i=0;i<r1;i++)
			{
				for (int j=0;j<c1;j++)
				{
					Console.Write(this.Data[i,j].ToString("N2")+"   " );				
				}
				Console.WriteLine(); 
			}
			Console.WriteLine(); 
		}

		/// <summary>
		/// Output the matrix to System.Console.
		/// </summary>
		/// <param name="format">The format specifier to display with.</param>
		public void Display(string format)
		{
			int r1 = this.Data.GetLength(0);int c1 = this.Data.GetLength(1);
			for (int i=0;i<r1;i++)
			{
				for (int j=0;j<c1;j++)
				{
					Console.Write(this.Data[i,j].ToString(format)+"   " );				
				}
				Console.WriteLine(); 
			}
			Console.WriteLine(); 
		}

		/// <summary>
		///  Calculates the inverse of the matrix.
		/// </summary>
		/// <returns>The inverse of the matrix.</returns>
		public Matrix Inverse()
		{
			if ((this.IsSquare)&&(!this.IsSingular))
			{
				return new Matrix(INV(this.Data));
			}
			else
			{
				throw new System.Exception (@"Cannot find inverse for non square /singular matrix"); 
			}
		}
		
		/// <summary>
		/// Transposes the rows and columns of the matrix.
		/// </summary>
		/// <returns> Transposed matrix.</returns>
		public Matrix Transpose()
		{
			double[,] D = transpose(this.Data) ;
			return new Matrix (D);
		}

		/// <summary>
		/// Creates a new matrix, setting each element to zero.
		/// </summary>
		/// <param name="size">The number of rows and columns for the matrix.</param>
		/// <returns>The newly created matrix.</returns>
		/// <remarks>Creates a square matrix of <paramref name="size"/>.</remarks>
		public static Matrix Zeros(int size)
		{
			double[,] D = new double[size,size];
			return new Matrix(D);
		}

		/// <summary>
		/// Creates a new matrix, setting each element to zero.
		/// </summary>
		/// <param name="rows">The number of rows.</param>
		/// <param name="cols">The number of columns.</param>
		/// <returns>The newly created matrix.</returns>
		public static Matrix Zeros(int rows, int cols)
		{
			double[,] D = new double[rows,cols];
			return new Matrix(D); 
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="COF"></param>
		/// <param name="CON"></param>
		/// <returns></returns>
		public Matrix LinSolve(Matrix COF, Matrix CON)
		{
			return COF.Inverse()*CON;		
		}

		/// <summary>
		/// Calculates the determinant of the matrix.
		/// </summary>
		/// <returns>The determinant of the matrix.</returns>
		public double Det()
		{
			if(this.IsSquare)
			{
				return det(this.Data);
			}
			else
			{
				throw new System.Exception ("Cannot Determine the DET for a non square matrix"); 
			}
		}

		#endregion

		#region Private Methods

		static double[,] INV (double[,] a )
		{
			int ro = a.GetLength(0);
			int co = a.GetLength(1);
			try
			{
				if (ro!=co)	{throw new System.Exception();}
				
			}
			catch{Console.WriteLine("Cannot find inverse for an non square matrix");}
			
			int q;double[,] b = new double[ro,co];double[,] I = eyes(ro);
			for(int p=0;p<ro;p++){for(q=0;q<co;q++){b[p,q]=a[p,q];}}			
			int i;double det=1;	
			if (a[0,0]==0)
			{
				i=1;
				while (i<ro)
				{
					if (a[i,0]!=0)
					{
						Matrix.interrow(a,0,i);		
						Matrix.interrow(I,0,i);
						det *= -1;
						break;
					}
					i++;
				}			
			}
			det*= a[0,0];
			Matrix.rowdiv(I,0,a[0,0]);
			Matrix.rowdiv(a,0,a[0,0]);
			for (int p=1;p<ro;p++)
			{
				q=0;
				while(q<p)
				{
					Matrix.rowsub(I,p,q,a[p,q]);
					Matrix.rowsub(a,p,q,a[p,q]);
					q++;
				}
				if(a[p,p]!=0)
				{
					det*=a[p,p];
					Matrix.rowdiv (I,p,a[p,p]); 
					Matrix.rowdiv (a,p,a[p,p]); 
					
				}
				if(a[p,p]==0)
				{
					for(int j=p+1;j<co;j++)
					{
						if(a[p,j]!=0)			// Column pivotting not supported
						{
							for(int p1=0;p1<ro;p1++)
							{
								for(q=0;q<co;q++)
								{
									a[p1,q]=b[p1,q];
								}
							}
							return inverse(b);
							  							
						}
					}
		
				}
			}
			for (int p=ro-1;p>0;p--)
			{
				for(q=p-1;q>=0;q--)
				{
					Matrix.rowsub (I,q,p,a[q,p]);
					Matrix.rowsub (a,q,p,a[q,p]);
				}
			}						
			for(int p=0;p<ro;p++)
			{
				for(q=0;q<co;q++)
				{
					a[p,q]=b[p,q];
				}
			}
			
			return(I);			
		}

		static double[,] inverse (double[,] a)
		{
			int ro = a.GetLength(0);
			int co = a.GetLength(1);
			double[,] ai = new double[ro,co];
			for (int p=0;p<ro;p++)
			{
				for (int q=0;q<co;q++)
				{
					ai[p,q]=0;
				}
			}
			try
			{
				if (ro!=co)
				{
					throw new System.Exception();

				}
			}
			catch
			{
				Console.WriteLine("Cannot find inverse for an non square matrix");		
				

			}
			double de = det(a);
			
			try
			{
				if (de==0)
				{
					System.Exception e1 = new Exception("Cannot Perform Inversion. Matrix Singular");
					
				}
			}
			catch(Exception e1)
			{
				Console.WriteLine ("Error:"+e1.Message );
			}
			
			
			for(int p=0;p<ro;p++)
			{
				for (int q=0;q<co;q++)
				{
					double [,] s = submat(a,p,q);
					double ds = det(s);
					ai[p,q]=Math.Pow(-1,p+q+2)*ds/de;
				
				}				
			}
			ai=transpose(ai);
			return(ai);
		}

		static void rowdiv(double[,] a,int r, double s )
		{
			int co=a.GetLength(1);
			for(int q=0;q<co;q++)
			{
				a[r,q]=a[r,q]/s;
			}
		}

		static void rowsub(double[,] a, int i, int j,double s)
		{
			int co=a.GetLength(1);
			for (int q=0;q<co;q++)
			{
				a[i,q]=a[i,q]-(s*a[j,q]);
			}
		}

		static  double[,] interrow (double[,]a ,int i , int j)
		{
			int ro = a.GetLength(0);
			int co = a.GetLength(1);
			double temp =0;
			for (int q=0;q<co;q++)
			{
				temp=a[i,q];
				a[i,q]=a[j,q];
				a[j,q]=temp;
			}
			return(a);
		}

		static double[,] eyes (int n)
		{
			double[,] a= new double[n,n];
			for (int p=0;p<n;p++)
			{
				for (int q=0;q<n;q++)
				{
					if(p==q)
					{
						a[p,q]=1;
					}
					else
					{
						a[p,q]=0;
					}
					
				}
			}
			return(a);
		}
		
		static double[,] scalmul(double scalar,double[,] A)
		{
			int ro = A.GetLength(0);
			int co = A.GetLength(1);
			double[,] B = new double[ro,co];
			for(int p=0;p<ro;p++)
			{
				for(int q=0;q<co;q++)
				{
					B[p,q]= scalar*A[p,q];
				}
			}
			return(B);	
		}
		
		static double det (double[,] a )
		{
			int q=0;
			int ro = a.GetLength(0);
			int co = a.GetLength(1);
			double[,] b = new double[ro,co];
			for(int p=0;p<ro;p++)
			{
				for(q=0;q<co;q++)
				{
					b[p,q]=a[p,q];
				}
			}
			int i=0;
			double det=1;
			try
			{
				if (ro!=co)
				{
					System.Exception E1 = new Exception("Error: Matrix Not Square");
					throw E1;
				}
			}
			catch(Exception E1)
			{
				Console.WriteLine (E1.Message );
			}
			try
			{
				if(ro==0)
				{
					System.Exception E2 = new Exception("Dimesion of the Matrix 0X0");
					throw E2;
				}
			}
			catch(Exception E2)
			{
				Console.WriteLine(E2.Message );
			}
			
			if(ro==2)
			{
				return( (a[0,0]*a[1,1]) - (a[0,1]*a[1,0]) ); 
			}
		
			if (a[0,0]==0)
			{
				i=1;
				while (i<ro)
				{
					if (a[i,0]!=0)
					{
						Matrix.interrow(a,0,i);		//Interchange of rows changes. determinent = determinent * -1
						det *= -1;
						break;
					}
					i++;
				}
			
			}
			if(a[0,0]==0)
			{
				return(0);							//If all the elements in a row or column of matrix are 0, determient is equal to 0
			}
			det*= a[0,0];
			Matrix.rowdiv(a,0,a[0,0]);
			for (int p=1;p<ro;p++)
			{
				q=0;
				while(q<p)							//preparring an upper triangular matrix
				{
					Matrix.rowsub(a,p,q,a[p,q]);
					q++;
				}
				if(a[p,p]!=0)
				{
					det*=a[p,p];					//Dividing the entire row with non zero diagonal element. Multiplying det with that factor.	
					Matrix.rowdiv (a,p,a[p,p]); 
				}
				if(a[p,p]==0)						// Chcek if the diagonal elements are zeros
				{
					for(int j=p+1;j<co;j++)
					{
						if(a[p,j]!=0)
						{
							Matrix.colsub(a,p,j,-1);//Adding of columns donot change the determinent
							
							det *= a[p,p];
							Matrix.rowdiv (a,p,a[p,p]);
							break;
						}
					}
		
				}
				if(a[p,p]==0)						//if diagonal element is still zero, Determinent is zero.
				{
					return(0);
				}
			
				
			}
			
			for(int p=0;p<ro;p++)
			{
				for(q=0;q<co;q++)
				{
					a[p,q]=b[p,q];
				}
			}
			return(det);
		}

		static double[,] submat(double [,] a, int ro, int co)
		{
			int n=a.GetLength(0);
			double [,] c = new double[n-1,n-1];
			int i=0;
			for(int p=0;p<n;p++)
			{				
				int j=0;
				if (p!= ro)
				{
					for(int q=0;q<n;q++)
					{
						if(q!=co)
						{
							c[i,j]=a[p,q];
							j+=1;
						}
					}
					i+=1;
				}
			}
	 		
			return(c);
		}
		
		static double[,] transpose(double[,] a)
		{
			
			int ro= a.GetLength(0);
			int co=a.GetLength (1);
			double[,] c = new double[co,ro];
			for(int p=0;p<ro;p++)
			{
				for(int q=0;q<co;q++)
				{
					c[q,p]=a[p,q];
				}
			
			}
					
			return(c);						
		
		}
		static void colsub(double[,]a,int i,int j,double s)
		{
			int ro = a.GetLength(0);
			int co = a.GetLength(1);
			for(int p=0;p<ro;p++)
			{
				a[p,i]=a[p,i]-(s*a[p,j]);
			}

		}

		#endregion

		/// <summary>
		/// The data contained within the matrix.
		/// </summary>
		public double[,] Data;

		private static Matrix sIdentity;
	}
}
