public class CityCollection {   
   string[] m_Cities = {"New York","Paris","London"};
   public IEnumerable<string> Reverse {
      get {
         for(int i=m_Cities.Length-1; i>= 0; i--)
            yield return m_Cities[i];         
      }
   }

   public static void Main (string[] args) {
      CityCollection cc = new CityCollection();
      foreach (string c in cc.Reverse)
         Console.WriteLine(c);
   }
}
