$(function () {
    $('.table').DataTable();

});

function CourseClick() {
    swal.fire({
        text: "Loading Course Data. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}