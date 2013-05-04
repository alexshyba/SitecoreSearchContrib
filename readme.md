<h2>Compatibility</h2>
Please note that this branch is compatible with Sitecore 6.4.x and 6.5.x releases.

<h2>How to build code and deploy the demo project</h2>
1. Under the root of the repository, create a folder called /references and copy the following DLLs from your local Sitecore install in there:
   - Lucene.Net.dll
   - Sitecore.Client.dll
   - Sitecore.Kernel.dll
   
2. Open both webroot_path.txt and serialization_path.txt and adjust the path accordingly.

3. Open the project -> Build Solution.

4. http://localhost/sitecore/admin/serialization.aspx -> toggle "master" -> Update {master}.

5. Sitecore -> Publish Site -> Smart Publish.

6. Control Panel -> Database -> Rebuild the Links Database (core, master, web).

<h2>How to run</h2>
1. Launch http://localhost/scripts/indexrebuilder.aspx and toggle "demo" rebuild.
   Alternatively, run Control Panel -> Rebuild the Search Index -> "demo".

3. Launch http://localhost/sitecore modules/web/searchdemo/
