<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Editor.aspx.cs" Inherits="LanguageRecognitionSystem.WorkingPages.Editor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Language Recognition System Editor</title>

    <script src="../Content/scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="../Content/scripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script type="../text/javascript" src="Content/bootstrap/js/bootstrap.js"></script>
    <script type="text/javascript" src="../Content/scripts/recognize-lang.js"></script>

    <link href="../Content/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="../Content/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="../Content/css/jquery-ui.css" rel="stylesheet" />
    <link href="../Content/css/textarea-styles.css" rel="stylesheet" />
    
</head>
<body>
    <div id="wrap">

        <div class="container">
            <div class="page-header">
                <h1>Language Revognition System</h1>
            </div>
            <p class="lead">Edit Redactor.</p>
        </div>
        <div id="push"></div>
    </div>
    <div id="footer" class="bottom hero-unit">
        <div class="container">
            <textarea class="textarea_style" id="sourceTextArea" cols="70" rows="5" title=""></textarea>
            <button id="btnRecognize" class="btn-large btn-success">Recognize</button>
        </div>
    </div>
</body>
</html>