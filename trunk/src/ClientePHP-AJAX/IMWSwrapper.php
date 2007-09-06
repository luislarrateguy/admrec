<?php
/*
 *	$Id: wsdlclient1.php,v 1.2 2004/03/15 23:06:17 snichol Exp $
 *
 *	WSDL client sample.
 *
 *	Service: WSDL
 *	Payload: document/literal
 *	Transport: http
 *	Authentication: none
 */
	require_once('nusoap/lib/nusoap.php');
	$proxyhost = isset($_POST['proxyhost']) ? $_POST['proxyhost'] : '';
	$proxyport = isset($_POST['proxyport']) ? $_POST['proxyport'] : '';
	$proxyusername = isset($_POST['proxyusername']) ? $_POST['proxyusername'] : '';
	$proxypassword = isset($_POST['proxypassword']) ? $_POST['proxypassword'] : '';
	$client = new soap_client('http://192.168.0.2:8084/index.asmx?wsdl=0', true,
							$proxyhost, $proxyport, $proxyusername, $proxypassword);

	switch ($_GET["method"]) {
		case "Conectar": 
				$param = array('nick' => $_GET['key']);
				$result = $client->call('Conectar', array('parameters' => $param), '', '', false, true);
				if (is_string($result["ConectarResult"])) {
					$result= $result["ConectarResult"] == "true";	
				} else {
					$result = array("ERROR");
				}
			break;
		case "Desconectar":
				$param = array('key' => $_GET['key']);
				$result = $client->call('Desconectar', array('parameters' => $param), '', '', false, true);
				if (is_string($result["DesconectarResult"])) {
					$result= $result["DesconectarResult"] == "true";	
				} else {
					$result = array("ERROR");
				}
			break;
		case "GetListaContactos":
				$param = array('key' => $_GET['key']);
				$result = $client->call('GetListaContactos', array('parameters' => $param), '', '', false, true);
				if (is_array($result["GetListaContactosResult"])) {
					if (is_array($result["GetListaContactosResult"]["anyType"])) {
						$result = $result["GetListaContactosResult"]["anyType"];
					} else {
						$result= array($result["GetListaContactosResult"]["anyType"]);
					}				
				} else {
					if ($result["GetListaContactosResult"] =="")
						$result = '';
					else
						$result = array("ERROR");
				}
			break;
		case "EnviarMensajeA":
				$param = array('key' =>$_GET['key'],'mensaje'=>$_GET['mensaje'],'nick'=>$_GET['nick']);
				$result = $client->call('EnviarMensajeA', array('parameters' => $param), '', '', false, true);
				if (is_string($result["EnviarMensajeAResult"])) {
					$result= $result["EnviarMensajeAResult"] == "true";	
				} else {
					$result = array("ERROR");
				}
			break;
		case "GetUltimosMensajesRecibidos":
				$param = array('key' => $_GET['key']);
				$result = $client->call('GetUltimosMensajesRecibidos', array('parameters' => $param), '', '', false, true);
				if (is_array($result["GetUltimosMensajesRecibidosResult"])) {
					if (is_array($result["GetUltimosMensajesRecibidosResult"]["anyType"])) {
						$result = $result["GetUltimosMensajesRecibidosResult"]["anyType"];
					} else {
						$result= array($result["GetUltimosMensajesRecibidosResult"]["anyType"]);
					}				
				} else {
					if ($result["GetUltimosMensajesRecibidosResult"] =="")
						$result = '';
					else
						$result = array("ERROR");
				}
			break;
	}
	echo json_encode($result);
?>