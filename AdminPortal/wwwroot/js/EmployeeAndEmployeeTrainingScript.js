

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
    $('.EmployeeFileAttachmentData').hover(function (event) {
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

function EmployeeDetailsView()
{
    swal.fire({
        text: "Loading Employee Data. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function SearchEmployee()
{
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

function EnrolEmployeeLoader()
{
    swal.fire({
        text: "Loading Course Data. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}