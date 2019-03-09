function duyetkhoahoc(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Duyetkhoahoc/"+id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function duyettutor(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Duyettutor/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function duyetparent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Duyetparent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function duyetstudent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Duyetstudent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}
//KHOA
function khoatutor(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Khoatutor/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function khoaparent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Khoaparent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}

function khoastudent(id) {
    //alert('!!id ' + id)
    var request = new XMLHttpRequest();
    var url = "/Home/Khoastudent/" + id;
    var method = "POST";
    request.open(method, url);
    request.send();
}