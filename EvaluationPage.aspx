<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EvaluationPage.aspx.cs" Inherits="EvaluationPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Video Quality Evaluation</title>
    <link href="Content/bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function myLoadScript() {
            document.getElementById("mainPanel").style.height = $(window).height() -50 + "px";
        };
    </script>
</head>
<body onload="myLoadScript();">
    <script src="Scripts/jquery-2.1.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <div id="mainPanel" class="panel panel-default">
        <div class="panel-heading">
            <h1 style="text-align:center">Video Selection Experiment</h1>
            <h4 style="text-align:center">Please select the best option by clicking the button on the right!</h4>
        </div>
        <div class="panel-body" style="height:80%;">
            <form id="frm" runat="server">
                <div class="row" id="VideosRow" style:"height:100%;">
                    <div class="col-md-2" style="height:100%;"></div>
                    <div class="col-md-4" style="height:100%;">
                        <table style="height:100%;width:100%;align-items:center;vertical-align:central;">
                            <tr>
                                <td>
                                    <video id="originalVideoID" runat="server" style="width:300px;vertical-align:central;" preload="auto" muted autoplay loop>
                                        <source src="" type="video/mp4" />
                                    </video>
                                    <div class="progress" style="width:300px;">
                                    <div class="progress-bar" role="progressbar" aria-valuenow="100%" style="width:100%;">
                                        <span style="text-align:center">Original Video</span> 
                                    </div>
                                </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-4" style="height:100%;align-items:center;">
                        <table style="height:100%;width:100%;align-items:center;">
                            <tr>
                                <td style="width:70%;">
                                        <video id="video1ID" runat="server" style="width:300px;vertical-align:central;" preload="auto" muted autoplay loop>
                                            <source src="a.mp4" type="video/mp4" />
                                        </video>
                                </td>
                                <td style="width:30%;">
                                        <asp:LinkButton ID="videoButton1" runat="server" AutoPostBack="TRUE" class="btn btn-success btn-lg" 
                                            style="vertical-align:middle;margin-left:20px;" onclick="SelectButtonOnClick">
                                        <span class="glyphicon glyphicon-ok"></span> Select
                                    </asp:LinkButton> 
                                </td>
                            </tr>
                            <tr>
                                <td style="width:70%;">
                                    <video id="video2ID" runat="server" style="width:300px;vertical-align:central;" preload="auto" muted autoplay loop>
                                            <source src="a.mp4" type="video/mp4" /> 
                                        </video>
                                </td>
                                <td style="width:30%;">
                                        <asp:LinkButton ID="videoButton2" runat="server" AutoPostBack="TRUE" class="btn btn-success btn-lg" 
                                            style="vertical-align:middle;margin-left:20px;" onclick="SelectButtonOnClick">
                                        <span class="glyphicon glyphicon-ok"></span> Select
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-2" style="height:100%"></div>
                </div>
            </form>
        </div>
        <div class="panel-footer">
            <h5 style="text-align:center">Cigdem Kocberber, Bogazici University, Turkey</h5>
        </div>
    </div>

</body>
</html>
