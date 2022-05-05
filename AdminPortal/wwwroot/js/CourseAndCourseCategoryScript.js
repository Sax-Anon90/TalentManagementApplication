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

function AddNewCourse() {
    swal.fire({
        text: "Adding new Course. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

var courseId;
$(function () {
    $('.CourseFileAttachmentData').hover(function (event) {
        var btn = $(this);
        courseId = btn.attr('data-courseId');
        $('#courseId').val(courseId);
    });
});
$(function () {
    $('.courseAttatchmentFileData').change(function (event) {
        swal.fire({
            text: "Uploading Course File Attatchment. Please wait ...",
            showConfirmButton: false,
            allowOutsideClick: false,
            didOpen: () => {
                swal.showLoading();
            }
        });
        document.getElementById('UploadCourseFileAttatchmentForm').submit();
    });
});

$(function () {
    var PlaceHolderElement = $('#CourseDetailsPlaceHolderHere');
    $('button[data-bs-toggle="modal viewModel"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

$(function () {
    var PlaceHolderElement = $('#UpdateCoursePlaceHolderHere');
    $('button[data-bs-toggle="modal updateModal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});



function UpdateCourse() {
    swal.fire({
        text: "Updating Course. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
};

$(function () {
    $(document).on('click', '.deleteCourseFile', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this course File attachment?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting course file attachment. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var courseFileAttachmentId = btn.attr('data-courseFileAttachmentId');
                $('#courseFileAttachmentId').val(courseFileAttachmentId);
                document.getElementById('DeleteCourseFileAttachmentForm').submit();
            }
        });
    });
});

$(function () {
    $(document).on('click', '.DeleteCoursebtn', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this course?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting course. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var courseId = btn.attr('data-Id');
                $('#Id').val(courseId);
                document.getElementById('DeleteCourseForm').submit();
            }
        });
    });
});

function AddNewCourseCategory() {
    swal.fire({
        text: "Adding new course category. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

$(function () {
    var PlaceHolderElement = $('#CourseCategoryUpdatePlaceHolderHere');
    $('button[data-bs-toggle="modal update"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

$(function () {
    var PlaceHolderElement = $('#CourseCategoryDetailsPlaceHolderHere');
    $('button[data-bs-toggle="modal viewModal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

function UpdateCourseCategory() {
    swal.fire({
        text: "Updating category Category. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}


$(function () {
    $(document).on('click', '.DeleteCourseCategorybtn', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this course Category? By Deleting this category you will also remove all other courses in this category",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting course Category. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var courseCategoryId = btn.attr('data-courseCategoryId');
                $('#courseCategoryId').val(courseCategoryId);
                document.getElementById('DeleteCourseCategoryForm').submit();
            }
        });
    });
});