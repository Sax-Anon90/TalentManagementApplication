function AddNewEmployee() {
    swal.fire({
        text: "Adding New Employee. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

var employeeId;
$(function () {
    $(document).on('mouseenter', '.EmployeeFileAttachmentData', function (event) {
        var btn = $(this);
        employeeId = btn.attr('data-employeeId');
        $('#employeeId').val(employeeId);
    });
});

$(function () {
    $('.EmployeeAttatchmentFileData').change(function (event) {
        swal.fire({
            text: "Uploading Employee File Attatchment. Please wait ...",
            showConfirmButton: false,
            allowOutsideClick: false,
            didOpen: () => {
                swal.showLoading();
            }
        });
        document.getElementById('UploadEmployeeFileAttatchmentForm').submit();
    });
});

function EmployeeDetailsView() {
    swal.fire({
        text: "Loading Employee Data. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function SearchEmployee() {
    swal.fire({
        text: "Checking. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

$(function () {
    var PlaceHolderElement = $('#courseEnrolPlaceHolderHere');
    $("body").on('click', 'button[data-bs-toggle="modal enrolModal"]', function (e) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

$(function () {
    var PlaceHolderElement = $('#UpdateEmployeeCourseEnrollmentPlaceHolderHere');
    $("body").on('click', 'button[data-bs-toggle="modal enrollmentModal"]', function (e) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

function EnrolCourseDetails() {
    swal.fire({
        text: "Loading Course Data. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function Enrol() {
    swal.fire({
        text: "Enrolling. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function UpdateEmployee() {
    swal.fire({
        text: "Updating Employee. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

$(function () {
    var PlaceHolderElement = $('#EmployeeUpdatePlaceHolderHere');
    $('body').on('click', 'button[data-bs-toggle="modal employeeModal"]', function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

$(function () {
    $(document).on('click', '.DeleteEmployeeFileAttachment', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this Employee's File attachment?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting employee file attachment. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var employeeFileAttachmentId = btn.attr('data-employeeFileAttachmentId');
                var employeeIdForDetails = btn.attr('data-employeeIdForDetails');
                $('#employeeFileAttachmentId').val(employeeFileAttachmentId);
                $('#employeeIdForDetails').val(employeeIdForDetails);
                document.getElementById('DeleteEmployeeFileAttachmentForm').submit();
            }
        });
    });
});


$(function () {
    $(document).on('click', '.DeleteEmployeebtn', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this employee?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting employee. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var employeeNo = btn.attr('data-employeeNo');
                $('#employeeNo').val(employeeNo);
                document.getElementById('DeleteEmployeeForm').submit();
            }
        });
    });
});

$(function () {
    $(document).on('click', '.unEnrollEmployee', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to unenroll this employee from this course?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Unenrolling. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var courseEnrollmentId = btn.attr('data-courseEnrollmentId');
                var employeeId = btn.attr('data-employeeId')
                $('#courseEnrollmentId').val(courseEnrollmentId);
                $('#employeeId').val(employeeId);
                document.getElementById('UnenrollEmployeeForm').submit();
            }
        });
    });
});

function UpdateEmployeeCourseEnrollment() {
    swal.fire({
        text: "Updating Employee Course Enrollment. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });

}

