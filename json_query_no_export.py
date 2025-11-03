import pyodbc
import json
from os.path import join

def export_to_json(data, file_path):
    # Write the JSON data to the file
    with open(file_path, 'w', encoding='utf-8') as json_file:
        json.dump(data, json_file, ensure_ascii=False, indent=4)
    print(f"Json data has been exported to {file_path}")

# Database connection parameters
server = 'sql2101-fm1-in.amr.corp.intel.com,3181'
database = 'pinfinder_production'
username = 'pinfinder_pr_rw'
password = '6OsW32m4x3x68Qx'

def header_query(lot_id_input, lcode_input, smrynam):
    # Connect to the database
    cnxn = pyodbc.connect('DRIVER={ODBC Driver 18 for SQL Server};SERVER=' + server +
                        ';DATABASE=' + database + ';UID=' + username + ';PWD=' + password)
    cursor = cnxn.cursor()

    # Query to retrieve header data from the database
    header_query = """
    SELECT
        stpl, soc, env, lotId, packg, prgnm, prdct, testertype, sspec, lcode, sysid, tempr, ldbid, smrynam, prdctName, uniqueFolderName, overflow_flag, iudesignid, mudesignid
    FROM
        [pinfinder_production].[dbo].[ituff_header]
    WHERE
        lcode = ? AND lotId = ? AND smrynam = ?
    """

    # Execute the header query
    cursor.execute(header_query, (lcode_input, lot_id_input, smrynam))

    # Fetch the header results
    header_results = cursor.fetchone()

    # Close the database connection
    cursor.close()
    cnxn.close()

    # Check if header results were found
    if header_results is None:
        return None  # Or return an empty dictionary, or any other appropriate value

    # Process the header results and add to JSON
    header_data = {
        "Header": [
            {"stpl": header_results.stpl},
            {"soc": header_results.soc},
            {"env": header_results.env},
            {"lotId": header_results.lotId},
            {"packg": header_results.packg},
            {"prgnm": header_results.prgnm},
            {"prdct": header_results.prdct},
            {"testertype": header_results.testertype},
            {"sspec": header_results.sspec},
            {"lcode": header_results.lcode},
            {"sysid": header_results.sysid},
            {"tempr": header_results.tempr},
            {"ldbid": header_results.ldbid},
            {"smrynam": header_results.smrynam},
            {"prdctName": header_results.prdctName},
            {"uniqueFolderName": header_results.uniqueFolderName},
            {"iudesignid": header_results.iudesignid},
            {"mudesignid": header_results.mudesignid},
            {"overflow_flag": header_results.overflow_flag},
        ]
    }
    return header_data
    
def data_query(lot_id_input, lcode_input, smrynam):
    # Connect to the database
    cnxn = pyodbc.connect('DRIVER={ODBC Driver 18 for SQL Server};SERVER=' + server +
                        ';DATABASE=' + database + ';UID=' + username + ';PWD=' + password)
    cursor = cnxn.cursor()

    # Query to retrieve unit header and unit data from the database
    data_query = """
    SELECT
        [uh].[site], [uh].[testNameAccuracy], [uh].[start], [uh].[tuiId], [uh].[end], [uh].[sum], [uh].[productNum], [uh].[ult], [uh].[fbin], [uh].[visualId], [uh].[eightdigitbin], [uh].[index], [uh].[smrynam],
        [ud].[Tname], [ud].[Die], [ud].[Connector], [ud].[FailCycle], [ud].[PinName], [ud].[TiuPinName], [ud].[MEASURE], [ud].[FailPattern], [ud].[CHAN], [ud].[FailVector]
    FROM
        [pinfinder_production].[dbo].[unit_header] AS [uh]
    JOIN
        [pinfinder_production].[dbo].[unit_data] AS [ud]
    ON
        [uh].[lcode] = [ud].[lcode] AND [uh].[lotId] = [ud].[lotId] AND [uh].[productNum] = [ud].[productNum] AND [uh].[smrynam] = [ud].[smrynam] AND [uh].[tuiId] = [ud].[tuiId] AND [uh].[testNameAccuracy] = [ud].[testNameAccuracy]
    WHERE
        [uh].[lcode] = ? AND [uh].[lotId] = ? AND [uh].[smrynam] = ?
    """

    # Execute the data query
    cursor.execute(data_query, (lcode_input, lot_id_input, smrynam))

    # Fetch the results
    results = cursor.fetchall()

    # Close the database connection
    cursor.close()
    cnxn.close()

    # Process the results and format as JSON
    data = {}
    for row in results:
        fbin_prefix = row.fbin[:2]
        product_num = str(row.productNum)  # Convert to string if not already
        tname = row.Tname

        # Initialize the nested structure if not present
        if fbin_prefix not in data:
            data[fbin_prefix] = []

        # Find or create the unitHeader entry
        unit_header_entry = next((item for item in data[fbin_prefix] if product_num in item), None)
        if not unit_header_entry:
            # Create a new entry for this product_num
            unit_header_entry = {
                product_num: {
                    "unitHeader": {
                        "site": row.site,
                        "testNameAccuracy": row.testNameAccuracy,
                        "start": row.start,
                        "tuiId": row.tuiId,
                        "end": row.end,
                        "sum": row.sum,
                        "productNum": product_num,
                        "ult": row.ult,
                        "fbin": row.fbin,
                        "visualId": row.visualId,
                        "eightdigitbin": row.eightdigitbin,
                    },
                    "unitData": []
                }
            }
            data[fbin_prefix].append(unit_header_entry)
            # Since we just created it, we know the structure is correct
            product_data = unit_header_entry[product_num]
        else:
            # Access the existing entry for this product_num
            product_data = unit_header_entry[product_num]

        # Find or create the tname entry within unitData
        tname_entry = next((item for item in product_data["unitData"] if tname in item), None)
        if not tname_entry:
            tname_entry = {tname: []}
            product_data["unitData"].append(tname_entry)

        # Append unit data to the tname list
        tname_entry[tname].append({
            "Die": row.Die,
            "Connector": row.Connector,
            "FailCycle": row.FailCycle,
            "PinName": row.PinName,
            "TiuPinName": row.TiuPinName,
            "MEASURE": row.MEASURE,
            "FailPattern": row.FailPattern,
            "CHAN": row.CHAN,
            "FailVector": row.FailVector,
        })

    return data

def download_json(lot_id_input, lcode_input, smrynam):
    header_data = header_query(lot_id_input, lcode_input, smrynam)
    if header_data is None: return header_data
    data = data_query(lot_id_input, lcode_input, smrynam)
    # Combine header and data into one JSON structure
    output_json = {
        "Header": header_data["Header"],
        "Misc": [
                {
                    "additionalAccessGroup": ""
                },
                {
                    "additionalAccessGroupLink": ""
                }
            ],
        **data
    }

    # Convert the combined dictionary to JSON
    json_data = json.dumps(output_json, indent=4)
    print(json_data)
    #export_to_json(output_json, join(r"C:\Temp\Ituff", lot_id_input + "_" + lcode_input + "_" + smrynam + "_true.json"))
    return json_data
    
# Inputs for the query
# lot_id_input = 'TE298562A'
# lcode_input = '6167'
# smrynam = '1A'

# download_json(lot_id_input, lcode_input, smrynam)