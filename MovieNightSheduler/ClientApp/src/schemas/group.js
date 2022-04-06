import * as Yup from "yup";

export const GroupSchema = Yup.object().shape({
    name: Yup.string()
        .min(4, "Group name must be over 4 characters long")
        .max(50, "Group name can not be over 50 characters long")
        .matches(/^[A-Za-z0-9]+$/, { message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
    description: Yup.string()
        .min(2, "Description must between 2-220 characters")
        .max(60, "Description must between 2-220 characters")
        .matches(/^[A-Za-z0-9!#$%&]+$/, { message: "use valid characters", excludeEmptyString: true })
        .required("required")
});