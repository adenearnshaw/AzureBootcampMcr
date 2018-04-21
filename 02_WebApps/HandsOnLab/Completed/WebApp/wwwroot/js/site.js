var clickCount = 0;
var myButton = document.getElementById('my-button');
onInit();

function onInit() {
    myButton.addEventListener('click', myButtonClicked);
    document.getElementById('clear-count-button').addEventListener('click', clearCount);

    window.addEventListener("beforeunload", function (event) {
        appInsights.trackMetric("User left", clickCount);
    });
}

function myButtonClicked() {
    clickCount++;
    console.log("Click count: " + clickCount);
    myButton.innerHTML = "You clicked me " + clickCount + " times!";
    appInsights.trackEvent("User Incremented Count");
    
}

function clearCount() {
    if (clickCount === 0)
        return;

    clickCount = 0;
    console.log("Click count reset");
    myButton.innerHTML = "Click me";
    appInsights.trackEvent("User Reset Count");
}