import * as Yup from "yup";

export const LoginSchema = Yup.object().shape({
    username: Yup.string()
        .min(2, "Username must be over 2 characters long")
        .max(50, "Username can not be over 50 characters")
        .matches(/^[A-Za-z0-9]+$/, { message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
    password: Yup.string()
        .min(2, "Password must be at least 2 characters long")
        .max(60, "Passwords are not over 60 characters")
        .matches(/[A-Za-z0-9!#$%&]+/, { message:"A-Za-z0-9!#$%&", excludeEmptyString: true })
        .required("Required")
});
