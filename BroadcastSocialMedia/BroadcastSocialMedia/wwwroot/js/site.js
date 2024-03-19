// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


document.addEventListener('DOMContentLoaded', function () {
    const switchThemeButton = document.getElementById('switchThemeButton');
    const navbar = document.getElementById('navbar');
    const navbarLinkContainer = document.getElementById('navbarLinksCointainer');
/*    const body = document.body;*/
    let currentTheme = 1
    switchThemeButton.addEventListener('click', function () {


        navbar.classList.remove('navbar_style' + currentTheme);
        navbarLinkContainer.classList.remove('navbarLinksCointainer_style' + currentTheme);

        currentTheme++;

        if (currentTheme > 5) {
            currentTheme = 1;
        }

        navbar.classList.add('navbar_style' + currentTheme);
        navbarLinkContainer.classList.add('navbarLinksCointainer_style' + currentTheme);
    });


});