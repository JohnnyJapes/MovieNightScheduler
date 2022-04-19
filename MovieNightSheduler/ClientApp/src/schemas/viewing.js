import * as Yup from "yup";

export const ViewingSchema = Yup.object().shape({
    title: Yup.string()
        .min(4, "Event name must be over 4 characters long")
        .max(100, "Event name can not be over 100 characters long")
        .matches(/^[A-Za-z0-9!#$%&:,]+$/, { message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
    description: Yup.string()
        .min(2, "Description must between 2-220 characters")
        .max(220, "Description must between 2-220 characters")
        .matches(/^[A-Za-z0-9!#$%&,]+$/, { message: "use valid characters", excludeEmptyString: true })
        .required("required"),
    movieTitle: Yup.string()
        .min(2, "Movie title must be over 2 characters long")
        .max(130, "Movie title can not be over 130 characters long")
        .matches(/^[A-Za-z0-9!#$%&:,]+$/, { message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
    date: Yup.date()
        .min(new Date(), "Date of event must be in the future")
        .required("Required"),
    location: Yup.string()
        .min(2, "Location must be over 2 characters long")
        .max(140, "Location can not be over 140 characters long")
        .matches(/^[A-Za-z0-9!#$%&:,]+$/, { message: "valid characters: letters, numbers", excludeEmptyString: true })
        .required("Required"),
});