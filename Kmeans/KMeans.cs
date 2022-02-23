namespace Kmeans
{
    public struct Point
    {
        public int X;
        public int Y;
        public int Cluster;
    }
    public class KMeans
    {
        Point[] MyPoints;
        Point[] MyCentroids;
        int PointNumber;
        int ClusterNumber;
        Random rand;

        public KMeans(Point[] myPoints, int pointNumber, int clusterNumber = 5)
        {
            MyPoints = myPoints;
            MyCentroids = new Point[1024];
            PointNumber = pointNumber;
            ClusterNumber = clusterNumber;
            rand = new Random(7524);
        }

        int MyCMP(Point a, Point b)
        {
            return a.Cluster - b.Cluster;
        }

        public void KMeans_Inital_RandomPartition()
        {
            for (int I = 0; I < PointNumber; I++)
            {
                MyPoints[I].Cluster = I % ClusterNumber;
            }
            KMeans_Update();
        }

        public void KMeans_Inital_Forgy()
        {
            int I, J, L;
            Point MyTemp;
            for (I = 0; I < PointNumber; I++)
            {
                L = rand.Next() % PointNumber;
                MyTemp = MyPoints[I];
                MyPoints[I] = MyPoints[L];
                MyPoints[L] = MyTemp;
            }

            for (I = 0; I < ClusterNumber; I++)
            {
                MyCentroids[I] = MyPoints[I];
            }
        }

        int KMeans_GeneratePoints(int N)
        {
            int I, J, K;
            for (I = 0; I < N & I < 1024; I++)
            {
                MyPoints[I].X = rand.Next() % 1000;
                MyPoints[I].Y = rand.Next() % 1000;
                MyPoints[I].Cluster = int.MaxValue;
            }
            return I;
        }

        //#define EUCLID(X,Y) ((X)*(X))+((Y)*(Y))

        int EUCLID(int x, int y)
        {
            return ((x) * (x)) + ((y) * (y));
        }

        int KMeans_Assign()
        {
            int I, J, K;
            int Min, Sum;

            for (I = 0, Sum = 0; I < PointNumber; I++)
            {
                Min = int.MaxValue;
                for (J = 0; J < ClusterNumber; J++)
                {
                    if (Min > EUCLID(MyPoints[I].X - MyCentroids[J].X, MyPoints[I].Y - MyCentroids[J].Y))
                    {
                        Min = EUCLID(MyPoints[I].X - MyCentroids[J].X, MyPoints[I].Y - MyCentroids[J].Y);
                        MyPoints[I].Cluster = J;
                    }
                }
                Sum += Min;
            }

            return Sum;
        }

        void KMeans_Update()
        {
            MyCentroids = MyPoints.GroupBy(p => p.Cluster).Select(g => new Point() { X = (int)g.Average(a => a.X), Y = (int)g.Average(b => b.Y) }).ToArray();
        }

        public Point[] KMeans_Algorithm()
        {
            int Sum, OldSum = int.MaxValue;
            //srand(time(NULL));
            //KMeans_Inital_RandomPartition(P,K);
            while (true)
            {
                Sum = KMeans_Assign();
                if (Sum == OldSum)
                {
                    break;
                }
                KMeans_Update();
                OldSum = Sum;
            }
            return MyPoints;
        }
    }
}