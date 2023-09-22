export function initialize()
{
    let idb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
    idb.onupgradeneeded = function ()
    {
        let db = idb.result;
        db.createObjectStore("forecasts", { keyPath: "date" });
    }
}

export function set(collectionName, value)
{
    let idb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    idb.onsuccess = function ()
    {
        let transaction = idb.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName)
        collection.put(value);
    }
}

export async function get(collectionName, id)
{
    let request = new Promise((resolve) =>
    {
        let idb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        idb.onsuccess = function ()
        {
            let transaction = idb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.get(id);

            result.onsuccess = function (e)
            {
                resolve(result.result);
            }
        }
    });

    let result = await request;

    return result;
}

export async function getAll(collectionName)
{
    let request = new Promise((resolve) =>
    {
        let idb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        idb.onsuccess = function ()
        {
            let transaction = idb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.getAll();

            result.onsuccess = function (e)
            {
                resolve(result.result);
            }
        }
    });

    let result = await request;

    return result;
}

let CURRENT_VERSION = 1;
let DATABASE_NAME = "serverdata";