<?php
/*
* COAPI V1 - HEX Inc.
* Simple API To Get\Send Data to\from Server and Manage it 
* Created By 'ShekoHex' @hex.inc
* File Description : This file to get hacks from array
* parm : 
- gethacks[boolean] -> if true , will get the full list, else will update it
*/
$response = array();
$hacks = ['notepad','cmd'];
header('Content-Type: application/json');
if(isset($_GET['gethacks'])) 
{
    if ($_GET['gethacks'])
	{
        $response['success'] = true;
        $response['message'] = "Done we get the list successfully.";
		$response['hacks'] = $hacks;
        echo json_encode($response);
    }
	else
	{
      $response["success"] = false;
      $response["message"] = "Oops! An error occurred.";
      echo json_encode($response);  
    }
 }
 
 else 
 {
    $response["success"] = false;
    $response["message"] = "Required field(s) is missing";
    echo json_encode($response);
 } 
?>