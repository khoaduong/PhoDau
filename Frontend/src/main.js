import { getTasks, createTask, toggleTask, deleteTask } from "./api/client.js";

const list = document.getElementById("tasks");
const form = document.getElementById("task-form");
const input = document.getElementById("title");

async function loadTasks() {
  list.innerHTML = "";
  const tasks = await getTasks();

  tasks.forEach(t => {
    const li = document.createElement("li");
    li.textContent = t.title;
    if (t.isCompleted) li.style.textDecoration = "line-through";

    li.onclick = async () => {
      await toggleTask(t.id);
      loadTasks();
    };

    const del = document.createElement("button");
    del.textContent = "X";
    del.onclick = async (e) => {
      e.stopPropagation();
      await deleteTask(t.id);
      loadTasks();
    };

    li.appendChild(del);
    list.appendChild(li);
  });
}

form.onsubmit = async (e) => {
  e.preventDefault();
  await createTask(input.value);
  input.value = "";
  loadTasks();
};

loadTasks();