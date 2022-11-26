var state1=false;
function pass() {
    if (state1) {
        document.getElementById('password').setAttribute('type', 'password');
      
        document.querySelectorAll('.fa-eye-slash')[0].classList.toggle('fa-eye');
       
        state1 = false;
        
    }
    else {
        document.getElementById('password').setAttribute('type', 'text');
        document.querySelectorAll('.fa-eye-slash')[0].classList.toggle('fa-eye');
        state1 = true;

    }
}
var state2 = false;
function pass2() {
    if (state2) {
        document.getElementById('password2').setAttribute('type', 'password');

        document.querySelectorAll('.fa-eye-slash')[1].classList.toggle('fa-eye');

        state2 = false;

    }
    else {
        document.getElementById('password2').setAttribute('type', 'text');
        document.querySelectorAll('.fa-eye-slash')[1].classList.toggle('fa-eye');
        state2 = true;

    }
}


var state3 = false;
function pass3() {
    if (state3) {
        document.getElementById('password3').setAttribute('type', 'password');

        document.querySelectorAll('.fa-eye-slash')[2].classList.toggle('fa-eye');

        state3 = false;

    }
    else {
        document.getElementById('password3').setAttribute('type', 'text');
        document.querySelectorAll('.fa-eye-slash')[2].classList.toggle('fa-eye');
        state3 = true;

    }
}