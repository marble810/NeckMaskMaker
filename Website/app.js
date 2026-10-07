const field = document.getElementById('listing-url');
const copy = document.getElementById('copy');
document.getElementById('add-vcc').href = `vcc://vpm/addRepo?url=${encodeURIComponent(field.value)}`;
copy.addEventListener('click', async () => {
  try {
    await navigator.clipboard.writeText(field.value);
    copy.textContent = '已复制';
  } catch {
    field.focus();
    field.select();
    copy.textContent = '请手动复制';
  }
});
