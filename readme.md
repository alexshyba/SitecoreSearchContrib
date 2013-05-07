<h1>Welcome to Sitecore Search Contrib project</h1>
<h2>Compatibility</h2>
The codebase is compatible with Sitecore 6.4.x and 6.5.x releases.
It should also work with 6.6.0, however the code will build with warnings due to deprecated methods in Lucene 2.9. The code is still expected to work as expected.
If you have any issues, please submit it here.
<h2>How to build code and deploy the demo project</h2>
1. Under the root of the repository, there is a folder called /references. Copy the following DLLs from your local Sitecore install in there:
   - Lucene.Net.dll
   - Sitecore.Client.dll
   - Sitecore.Kernel.dll

2. Open both webroot_path.txt and serialization_path.txt and adjust the path accordingly.

3. Open the project -> Build Solution.
   If you do not want the demo stuff, adjust your Build Configuration accordingly.

If you want the demo stuff:

4. http://localhost/sitecore/admin/serialization.aspx -> toggle "master" -> Update {master}.

5. Sitecore -> Publish Site -> Smart Publish.

6. Control Panel -> Database -> Rebuild the Links Database (core, master, web).

7. Launch http://localhost/scripts/indexrebuilder.aspx and toggle "demo" rebuild.
   Alternatively, run Control Panel -> Rebuild the Search Index -> "demo".

8. Launch http://localhost/sitecore modules/web/searchdemo/
9. Enjoy!
