function AddNewUser() {
    swal.fire({
        text: "Adding User. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function AssignRolesToUserView() {
    swal.fire({
        text: "Loading roles. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function UserDetailsView() {
    swal.fire({
        text: "Loading user details. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function AssignRolesToUser() {
    swal.fire({
        text: "Assigning Roles to user. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

$(function () {
    $(document).on('click', '.deleteUserRolebtn', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this user's role?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting user role. Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var userRoleId = btn.attr('data-userRoleId');
                $('#userRoleId').val(userRoleId);
                document.getElementById('DeleteUserRoleForm').submit();
            }
        });
    });
});

$(function () {
    var PlaceHolderElement = $('#updateUserPlaceHolderHere');
    $("body").on('click', 'button[data-bs-toggle="modal userUpdateModal"]', function (e) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show')
        });
    });
});

function UpdateUser() {
    swal.fire({
        text: "updating user details. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

$(function () {
    $(document).on('click', '.DeleteUserbtn', function (event) {
        swal.fire({
            title: "Are you sure?",
            text: "Are you sure you want to delete this user?",
            icon: "warning",
            showCancelButton: true,
        }).then((result) => {
            if (result.isConfirmed) {
                swal.fire({
                    text: "Deleting user Please wait ...",
                    showConfirmButton: false,
                    allowOutsideClick: false,
                    didOpen: () => {
                        swal.showLoading();
                    }
                });
                var btn = $(this);
                var userId = btn.attr('data-userId');
                $('#userId').val(userId);
                document.getElementById('DeleteUserForm').submit();
            }
        });
    });
});

function CheckIfUserExists() {
    swal.fire({
        text: "Checking. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function Register() {
    swal.fire({
        text: "Registering. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}

function Login() {
    swal.fire({
        text: "Checking. Please wait ...",
        showConfirmButton: false,
        allowOutsideClick: false,
        didOpen: () => {
            swal.showLoading();
        }
    });
}