import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import HomeView from "../views/HomeView.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/404",
    component: () => import("@/views/error/NotFoundView.vue"),
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  },
  {
    path: "/auth/login",
    component: () => import("../views/auth/LoginView.vue"),
  },
  {
    path: "/auth/register",
    component: () => import("../views/auth/RegisterView.vue"),
  },
  {
    path: "/auth/unauthorized",
    component: () => import("../views/auth/UnauthorizedView.vue"),
  },
  {
    path: "/categories/:id",
    component: () => import("../views/Courses/CategoryView.vue"),
  },
  {
    path: "/categories",
    component: () => import("../views/Courses/AllCategoriesView.vue"),
  },

  {
    path: "/course/new",
    component: () => import("../views/Courses/NewCourseView.vue"),
  },
  // {
  //   path: "/courses/:categoryId",
  //   component: () => import("../views/All.vue"),
  // },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  if (to.matched.length < 1) {
    next(false);
    router.push("/404");
  }

  if (from.path == "/auth/login") next();
  const publicPages = [
    "/auth/login",
    "/auth/register",
    "/auth/unauthorized",
    "/",
    "/404",
  ];
  const authRequired = !publicPages.includes(to.path);
  const loggedIn = localStorage.getItem("user");

  // trying to access a restricted page + not logged in
  // redirect to login page
  if (authRequired && !loggedIn) {
    next("/auth/unauthorized");
  } else {
    next();
  }
});

export default router;
