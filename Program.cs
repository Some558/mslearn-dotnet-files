using Newtonsoft.Json;

// CurrentDirectory（現在のフォルダ）をcurrentDirectoryに代入
var currentDirectory = Directory.GetCurrentDirectory();
// Combineによって現在のフォルダとstoresを繋げて、storesDirectoryに代入する
var storesDirectory = Path.Combine(currentDirectory, "stores");
// Combineによって、現在のフォルダとsalesTotalDirをつなげて、salesTotalDirに代入する
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
// 先ほど指定したパスにsalesTotalDirフォルダを作成する
Directory.CreateDirectory(salesTotalDir);
// FindFilesメソッドにstoresDirectoryを使用し、戻り値をsalesFilesに代入する
// 戻り値の中身は、salesFilesの中で拡張子がjsonのものである。
var salesFiles = FindFiles(storesDirectory);
// CalculateSalesTotalメソッドに先ほど受けたjsonファイルが集まったsalesFilesを代入する
// 総売上の値が以下に入る
var salesTotal = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

// FindFilesメソッドでは、引数で受けたフォルダ名（salesFiles）に以下処理を加えてsalesFilesとして返す
IEnumerable<string> FindFiles(string folderName)
{
    // salesFiles変数をリストstring形式で生成
    List<string> salesFiles = new List<string>();
    // storesDirectroyの全てのディレクトリを検索し、それをfoundFilesに代入する
    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);
    // 先ほど見つかったfouncFilesをfileとして分割し以下処理を順に行う
    foreach (var file in foundFiles)
    {
        // fileの拡張子を入手し、extensionに代入する
        var extension = Path.GetExtension(file);
        // もし拡張子がjsonであれば、salesFilesに代入する
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    // salesTotalを0にする
    double salesTotal = 0;

    // salesFile(jsonファイルの集合体)をfileとして分割して順に処理する
    foreach (var file in salesFiles)
    {
        // fileの中身を読み、それをsalesJsonに代入する
        string salesJson = File.ReadAllText(file);
        // salesJsonをSalesData型に変換してdataに値を代入する
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        // dataに数値が入っていたら、salesTotalに追加する
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

record SalesData(double Total);