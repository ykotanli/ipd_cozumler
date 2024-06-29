private void button1_Click(object sender, EventArgs e)
{
    Assembly myDll =
   Assembly.LoadFrom(@"C:\Users\AHMET\source\repos\ClassLi
brary2\ClassLibrary2\bin\Debug\ClassLibrary2.dll");
    Type tipi =
   myDll.GetType("ClassLibrary2.DortIslem");
    MethodInfo[] methods = tipi.GetMethods();
    MethodInfo mtCikar = tipi.GetMethod("Cikar");
    object[] argstopass = new object[] {
Convert.ToInt32("52"), Convert.ToInt32("88") };
    object instance =
   Activator.CreateInstance(tipi);
    object res =
   methods[0].Invoke(instance, argstopass);
    object res2 = mtCikar.Invoke(instance,
   argstopass);
    MessageBox.Show(res.ToString());
}