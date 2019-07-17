let circle = document.getElementById("epaAQI");
let circleNumber = circle.innerText;
debugger;
if (circleNumber <= 50 && circleNumber >= 0) {
    circle.style.backgroundColor = "green";
}
else if (circleNumber <= 100 && circleNumber >= 51) {
    circle.style.backgroundColor = "yellow";
}
else if (circleNumber <= 150 && circleNumber >= 101) {
    circle.style.backgroundColor = "orange";
}
else if (circleNumber <= 200 && circleNumber >= 151) {
    circle.style.backgroundColor = "red";
}
else if (circleNumber <= 250 && circleNumber >= 101) {
    circle.style.backgroundColor = "rgb(102, 0, 128)";
}
else if (circleNumber <= 250 && circleNumber >= 101) {
    circle.style.backgroundColor = "rgb(102, 0, 128)";
}
else {
    circle.style.backgroundColor = "red";
}