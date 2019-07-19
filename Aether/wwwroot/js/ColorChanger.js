
function getColor(elementID) {
    let circle = document.getElementById(elementID);
    let circleNumber = circle.innerText;

    if (circleNumber <= 50 && circleNumber >= 0) {
        circle.style.backgroundColor = "rgba(27, 115, 48, 0.8)";
    }
    else if (circleNumber <= 100 && circleNumber >= 51) {
        circle.style.backgroundColor = "rgba(255,255,0,0.8)";
    }
    else if (circleNumber <= 150 && circleNumber >= 101) {
        circle.style.backgroundColor = "rgba(243, 156, 18, 0.08)";
    }
    else if (circleNumber <= 200 && circleNumber >= 151) {
        circle.style.backgroundColor = "rgba(255,0,0,0.8)";
    }
    else if (circleNumber <= 250 && circleNumber >= 101) {
        circle.style.backgroundColor = "rgba(102, 0, 128,0.8)";
    }
    else if (circleNumber <= 250 && circleNumber >= 101) {
        circle.style.backgroundColor = "rgba(102, 0, 128, 0.08)";
    }
    else {
        circle.style.backgroundColor = "rgba(255,0,0,0.8)";
    }
}
