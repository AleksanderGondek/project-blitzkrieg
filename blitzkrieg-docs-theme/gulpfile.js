var gulp = require('gulp');
var bower = require('gulp-bower');
var clean = require('gulp-clean');
var gutil = require('gulp-util');
var minifyCss = require('gulp-minify-css');

const THEME_NAME = 'blitzkrieg-docs';

const BOWER_PACKAGES_DIRECTORY = './bower_components';
const DEPENDENCIES_DIRECTORY = './assets/themes/' + THEME_NAME;
const SOURCE_FILES_DIRECTORY = './source';

gulp.task('clean-assets', function() {
    gutil.log('Cleaning assets..');
    return gulp.src(DEPENDENCIES_DIRECTORY+'/*', {read: false})
        .pipe(clean());
    gutil.log('Assets cleaned.');
});

gulp.task('copy-bootstrap', function() {
    gutil.log('Coping bootstrap files..');
    gulp.src(BOWER_PACKAGES_DIRECTORY + '/bootstrap/dist/**/*.min.*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/bootstrap'));
    return gulp.src(BOWER_PACKAGES_DIRECTORY + '/bootstrap/dist/fonts/*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/bootstrap/fonts'));
    gutil.log('Bootstrap files copied.');
});

gulp.task('copy-bootswatch-darkly', function() {
    gutil.log('Coping bootswatch-darkly files..');
    return gulp.src(BOWER_PACKAGES_DIRECTORY + '/bootswatch/darkly/*.min.*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/bootswatch-darkly/css'));
    gutil.log('Bootswatch-darkly files copied.');
});

gulp.task('copy-fontawesome', function() {
    gutil.log('Coping fontawesome files..');
    gulp.src(BOWER_PACKAGES_DIRECTORY + '/font-awesome/css/*.min.*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/fontawesome/css'));
    return gulp.src(BOWER_PACKAGES_DIRECTORY + '/font-awesome/fonts/*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/fontawesome/fonts'));
    gutil.log('Fontawesome files copied.');
});

gulp.task('copy-jquery', function() {
    gutil.log('Coping jquery files..');
    return gulp.src(BOWER_PACKAGES_DIRECTORY + '/jquery/dist/*.min.*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/jquery/js'));
    gutil.log('Jquery files copied.');
});

gulp.task('copy-custom-css', function() {
    gutil.log('Coping custom css files..');
    return gulp.src(SOURCE_FILES_DIRECTORY + '/css/*')
        .pipe(minifyCss({compatibility: 'ie8'}))
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY +
                        '/' + THEME_NAME +
                        '/' + 'css'));
    gutil.log('Css files copied.');
});

gulp.task('copy-custom-js', function() {
    gutil.log('Coping custom js files..');
    return gulp.src(SOURCE_FILES_DIRECTORY + '/js/*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY +
                        '/' + THEME_NAME +
                        '/' + 'js'));
    gutil.log('Js files copied.');
});

gulp.task('copy-custom-img', function() {
    gutil.log('Coping custom image files..');
    return gulp.src(SOURCE_FILES_DIRECTORY + '/img/*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY +
                        '/' + THEME_NAME +
                        '/' + 'img'));
    gutil.log('Image files copied.');
});

gulp.task('copy-custom-fonts', function() {
    gutil.log('Coping custom fonts files..');
    return gulp.src(SOURCE_FILES_DIRECTORY + '/fonts/*')
        .pipe(gulp.dest(DEPENDENCIES_DIRECTORY + '/blitzkrieg-docs/fonts'));
    gutil.log('Custom fonts files copied.');
});

gulp.task('copy-assets', ['copy-bootstrap', 'copy-bootswatch-darkly',
    'copy-fontawesome', 'copy-custom-css', 'copy-custom-js', 'copy-custom-img',
    'copy-custom-fonts', 'copy-jquery']);

gulp.task('bower', function() {
    return bower();
});

gulp.task('default', ['bower', 'clean-assets', 'copy-assets']);
