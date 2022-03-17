import * as Yup from "yup";

const RegisterSchema = Yup.object().shape({
    username: Yup.string()
        .min(2, "Username must be over 2 characters long")
        .max(50, "Username can not be over 50 characters")
        .matches(/^[A-Za-z0-9]+$/, {message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
    password: Yup.string()
        .min(2, "Password must between 2-60 characters")
        .max(60, "Password must between 2-60 characters")
        .matches(/^[A-Za-z0-9!#$%&]+$/, { message: "use valid characters", excludeEmptyString: true })
        .required("required")
});