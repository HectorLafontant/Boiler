var achievement_id = -1;
var last_id = last_achievement_id = achievement_id;

var ready = (callback) => {
    if (document.readyState != "loading") {
        callback();
    }
    else
    {
        document.addEventListener("DOMContentLoaded", callback);
    }
}

ready(() => {
    const btn = document.getElementById("add_achievement");
    btn.addEventListener("click", function() {
        AddAchievement();
    });
});

function AddAchievement(){
    console.log(last_achievement_id);
    var last_input_field = document.getElementById("achievement:" + last_achievement_id.toString());
    console.log(last_input_field);
    if ((last_input_field == null || last_input_field.value == "") && last_achievement_id != -1){
        console.log("return!");
        return;
    }
    if (last_achievement_id == -1){
        var remove_achievement_button = document.createElement("button");
        remove_achievement_button.setAttribute("type", "button");
        remove_achievement_button.classList.add("btn");
        remove_achievement_button.classList.add("btn-danger");
        remove_achievement_button.textContent = "Remove achievement"
        remove_achievement_button.id = "remove_achievement_button"
        remove_achievement_button.addEventListener("click", function() {
            RemoveAchievement();
        });
        document.getElementById("remove_button_container").appendChild(remove_achievement_button);
    }
    var input_field = document.createElement("input");
    input_field.setAttribute("type", "text");
    input_field.classList.add("form-control");
    input_field.classList.add("mb-1");
    input_field.required = true;
    
    achievement_id++;
    last_achievement_id = achievement_id;
    input_field.id = "achievement:" + achievement_id.toString();
    input_field.name = "AchievementsNames[" + achievement_id + "]";
    input_field.value = "";
    document.getElementById("achievements_container").appendChild(input_field);
    console.log(last_achievement_id);
}

function RemoveAchievement(){
    console.log("remove");
    document.getElementById("achievement:" + last_achievement_id.toString()).remove();
    achievement_id--;
    last_achievement_id = achievement_id;
    if(achievement_id == -1){
        document.getElementById("remove_achievement_button").remove();
    }
}