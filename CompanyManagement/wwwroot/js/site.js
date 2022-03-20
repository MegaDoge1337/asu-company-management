function CheckStringOnNullOrEmpty(value)
{
    return ((value !== null) && (value !== ""));
}

function CheckEmployeeAge(birthDate) {
    let date = Date.parse(birthDate);
    let now = Date.now();
    let differenceInMilliseconds = now - date;

    if (differenceInMilliseconds < 0) {
        return false;
    }

    let differenceInDays = Math.ceil(differenceInMilliseconds / (1000 * 60 * 60 * 24));
    if (differenceInDays < 6750) {
        return false;
    }

    return true;
}

function CheckEmployeeForm(event) {
    event.preventDefault()

    let form = $("#asu-form");

    let cipher = $("#cipher");
    let fullName = $("#fullName");
    let birthDate = $("#birthDate");
    let gender = $("#gender");
    let error = $("#asu-error");

    let isFieldsNotEmptyOrNull = CheckStringOnNullOrEmpty(cipher.val())
        && CheckStringOnNullOrEmpty(fullName.val())
        && CheckStringOnNullOrEmpty(birthDate.val())
        && CheckStringOnNullOrEmpty(gender.val());

    if (isFieldsNotEmptyOrNull) {
        let IsValidAge = CheckEmployeeAge(birthDate.val());
        if (IsValidAge) {
            form[0].submit();
            return true;
        }
        error.text("Ошибка: сотрудник не подходит по возрасту.");
        error.slideDown();
        return false;
    } else {
        error.text("Ошибка: не все поля формы заполнены.");
        error.slideDown();
        return false;
    }
}

function CheckJobForm(event) {
    event.preventDefault()

    let form = $("#asu-form");

    let title = $("#title");
    let salary = $("#salary");
    let rate = $("#rate");
    let employmentDate = $("#employmentDate");
    let employee = $("#employee");
    let error = $("#asu-error");

    let isFieldsNotEmptyOrNull = CheckStringOnNullOrEmpty(title.val())
        && CheckStringOnNullOrEmpty(salary.val())
        && CheckStringOnNullOrEmpty(rate.val())
        && CheckStringOnNullOrEmpty(employmentDate.val())
        && CheckStringOnNullOrEmpty(employee.val());

    if (isFieldsNotEmptyOrNull) {
        form[0].submit();
        return true;
    } else {
        error.text("Ошибка: не все поля формы заполнены.");
        error.slideDown();
        return false;
    }
}

function CheckPayoutForm(event) {
    event.preventDefault()

    let form = $("#asu-form");

    let job = $("#job");
    let payoutDate = $("#payoutDate");
    let error = $("#asu-error");

    let isFieldsNotEmptyOrNull = CheckStringOnNullOrEmpty(job.val())
        && CheckStringOnNullOrEmpty(payoutDate.val());

    if (isFieldsNotEmptyOrNull) {
        form[0].submit();
        return true;
    } else {
        error.text("Ошибка: не все поля формы заполнены.");
        error.slideDown();
        return false;
    }
}

function CheckYearInStatementOfChargesReport(event) {
    event.preventDefault()

    let form = $("#asu-form");

    let year = $("#year");
    let error = $("#asu-error");

    let isFieldsNotEmptyOrNull = CheckStringOnNullOrEmpty(year.val());

    if (isFieldsNotEmptyOrNull && year.val() !== "0") {
        form[0].submit();
        return true;
    } else {
        error.text("Ошибка: год не может быть нулевым.");
        error.slideDown();
        return false;
    }
}

function ToggleFiredInfoFields(event) {
    let firedInfoFields = $("#asu-fired-info");
    if (event.currentTarget.checked) {
        firedInfoFields.slideDown();
    }
    else {
        firedInfoFields.slideUp();
    }
}