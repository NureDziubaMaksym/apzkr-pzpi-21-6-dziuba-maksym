import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';

const resources = {
    en: {
        translation: {
            "User Interaction Menu": "User Interaction Menu",
            "Get All Users": "Get All Users",
            "Get User by Login": "Get User by Login",
            "Get User by ID": "Get User by ID",
            "Delete User": "Delete User",
            "Username": "Username",
            "Password": "Password",
            "Login": "Login",
            "Do not have an account?": "Do not have an account?",
            "Register": "Register",
            "Welcome": "Welcome",
            "Email": "Email",
            "Phone Number": "Phone Number",
            "Change Password": "Change Password",
            "Hide Password Change Form": "Hide Password Change Form",
            "Current Password": "Current Password",
            "New Password": "New Password",
            "Confirm New Password": "Confirm New Password",
            "Sessions Interaction Menu": "Sessions Interaction Menu",
            "Get All Sessions": "Get All Sessions",
            "Get Sessions by Focus Group ID": "Get Sessions by Focus Group ID",
            "Get Session Analysis Results": "Get Session Analysis Results",
            "Add New Session": "Add New Session",
            "Focus Group Interaction Menu": "Focus Group Interaction Menu",
            "Get All Focus Groups": "Get All Focus Groups",
            "Get Users by Group ID": "Get Users by Group ID",
            "Add User to Focus Group": "Add User to Focus Group",
            "Remove User from Focus Group": "Remove User from Focus Group",
            "Add New Focus Group": "Add New Focus Group",
            "Delete Focus Group": "Delete Focus Group",
            "Content Interaction Menu": "Content Interaction Menu",
            "Get all content": "Get all content",
            "Add new content": "Add new content",
            "Get user reactions to content": "Get user reactions to content",
            "Get content analysis results": "Get content analysis results",
            "Users": "Users",
            "Groups": "Groups",
            "Contents": "Contents",
            "Sessions": "Sessions",
            "Logout": "Logout"
            // Add other strings here
        }
    },
    ua: {
        translation: {
            "User Interaction Menu": "Меню взаємодії з користувачами",
            "Get All Users": "Отримати всіх користувачів",
            "Get User by Login": "Отримати користувача за логіном",
            "Get User by ID": "Отримати користувача за ID",
            "Delete User": "Видалити користувача",
            "Username": "Ім'я користувача",
            "Password": "Пароль",
            "Login": "Вхід",
            "Do not have an account?": "Не маєте облікового запису?",
            "Register": "Реєстрація",
            "Welcome": "Ласкаво просимо",
            "Email": "Електронна пошта",
            "Phone Number": "Номер телефону",
            "Change Password": "Змінити пароль",
            "Hide Password Change Form": "Сховати форму зміни пароля",
            "Current Password": "Поточний пароль",
            "New Password": "Новий пароль",
            "Confirm New Password": "Підтвердіть новий пароль",
            "Sessions Interaction Menu": "Меню взаємодії з сесіями",
            "Get All Sessions": "Отримати всі сесії",
            "Get Sessions by Focus Group ID": "Отримати сесії за ID фокус-групи",
            "Get Session Analysis Results": "Отримати результати аналізу сесії",
            "Add New Session": "Додати нову сесію",
            "Focus Group Interaction Menu": "Меню взаємодії з фокус-групами",
            "Get All Focus Groups": "Отримати всі фокус-групи",
            "Get Users by Group ID": "Отримати користувачів за ID групи",
            "Add User to Focus Group": "Додати користувача до фокус-групи",
            "Remove User from Focus Group": "Видалити користувача з фокус-групи",
            "Add New Focus Group": "Додати нову фокус-групу",
            "Delete Focus Group": "Видалити фокус-групу",
            "Content Interaction Menu": "Меню взаємодії з контентом",
            "Get all content": "Отримати весь контент",
            "Add new content": "Додати новий контент",
            "Get user reactions to content": "Отримати реакції користувачів на контент",
            "Get content analysis results": "Отримати результати аналізу контенту",
            "Users": "Користувачі",
            "Groups": "Групи",
            "Contents": "Контент",
            "Sessions": "Сесії",
            "Logout": "Вихід"
            // Add other strings here
        }
    }
};

i18n
    .use(initReactI18next)
    .init({
        resources,
        lng: "en", // Default language
        fallbackLng: "en",
        interpolation: {
            escapeValue: false
        }
    });

export default i18n;
